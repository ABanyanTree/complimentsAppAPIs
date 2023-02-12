using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DelphianLMS.ModelGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string strProjectPath = ConfigurationManager.AppSettings["ProjectPath"].ToString();
                txtDomainPath.Text = ConfigurationManager.AppSettings["DomainPath"].ToString();
                txtDomainNamespace.Text = ConfigurationManager.AppSettings["DomainNamespace"].ToString();
                txtDomainRepoPath.Text = ConfigurationManager.AppSettings["DomainRepoPath"].ToString();
                txtDomainRepoNameSpace.Text = ConfigurationManager.AppSettings["DomainRepoNamespace"].ToString();
                txtDataConstPath.Text = ConfigurationManager.AppSettings["DataConstantsPath"].ToString();
                txtDataConstNamespace.Text = ConfigurationManager.AppSettings["DataConstantsNamespace"].ToString();
                txtDomainInterfacePath.Text = ConfigurationManager.AppSettings["DomainInterfacesPath"].ToString();
                txtDomainInterfacesNamespace.Text = ConfigurationManager.AppSettings["DomainInterfacesNamespace"].ToString();
                txtDapperNamespace.Text = ConfigurationManager.AppSettings["DapperNamespace"].ToString();
                txtRepoClassPath.Text = ConfigurationManager.AppSettings["RepositoryClassPath"].ToString();
                txtRepoClassNamespace.Text = ConfigurationManager.AppSettings["RepositoryClassNamespace"].ToString();
                txtServiceInterfacePath.Text = ConfigurationManager.AppSettings["ServiceInterfacesPath"].ToString();
                txtServiceInterfaceNamespace.Text = ConfigurationManager.AppSettings["ServiceInterfacesNamespace"].ToString();
                txtServiceClassPath.Text = ConfigurationManager.AppSettings["ServiceClassPath"].ToString();
                txtServiceClassNamespace.Text = ConfigurationManager.AppSettings["ServiceClassNamespace"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                CreateSP();

                if (txtTableName.Text == string.Empty || txtDomainName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter details");
                    return;
                }

                List<TableDesign> lstTableCols = new List<TableDesign>();
                string strTable = txtTableName.Text;
                string strModel = txtDomainName.Text;
                string strRepoName = string.Empty;

                DataSet ds = GetDataSet(strTable);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TableDesign item = new TableDesign();
                    item.Column_Name = row["Column Name"].ToString();
                    item.Data_Type = row["Data Type"].ToString();
                    item.Character_Maximum_Length = Convert.ToInt32(row["Max Length"]);
                    lstTableCols.Add(item);
                }


                string strModelPath = txtDomainPath.Text;

                if (strModelPath == string.Empty)
                {
                    MessageBox.Show("Select Path");
                    return;
                }

                CreateDomainClasses(lstTableCols, strModel, strModelPath);
                CreateDomainConstants(lstTableCols, strModel, txtDataConstPath.Text);
                CreateDomainInterfaces(lstTableCols, strModel, txtDomainInterfacePath.Text);
                CreateRepositoryClasses(lstTableCols, strModel, txtRepoClassPath.Text);
                CreateServiceInterfaces(lstTableCols, strModel, txtServiceInterfacePath.Text);
                CreateServiceClasses(lstTableCols, strModel, txtServiceClassPath.Text);
                SetDI(strModel);

                MessageBox.Show("Done !!!", "Process Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR OCCURED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateSP()
        {
            try
            {
                string _strConnString = string.Empty;
                SqlCommand _sqlcmd = new SqlCommand();
                SqlParameter _sqlpara;
                SQLObject _sqlObject = new SQLObject();
                _strConnString = _sqlObject.GetMasterDBConnString();

                string cmdText = "";
                cmdText += " CREATE PROCEDURE [dbo].[sproc_GetTableDesign] ";
                cmdText += " @TableName varchar(100) ";
                cmdText += " AS ";
                cmdText += " BEGIN ";
                cmdText += " SET NOCOUNT ON; ";
                cmdText += " SELECT column_name as 'Column Name', data_type as 'Data Type',ISNULL(character_maximum_length,0) as 'Max Length' FROM information_schema.columns WHERE table_name = @TableName ";
                cmdText += " END ";

                _sqlcmd.CommandText = "sp_executesql";
                _sqlpara = new SqlParameter("@statement", cmdText);
                _sqlcmd.Parameters.Add(_sqlpara);

                _sqlObject.ExecuteNonQuery(_sqlcmd, _strConnString);
            }
            catch
            {

            }
        }

        private void CreateDomainClasses(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                StringBuilder content = new StringBuilder();
                string Constant = string.Empty;

                //write namespace
                content.AppendFormat("using System;\n", Environment.NewLine);

                //wrapper 
                content.AppendFormat("namespace " + txtDomainNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat(" public class " + strModelName + " : BaseEntity \n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);

                //write 
                foreach (TableDesign item in lst)
                {
                    string Name = string.Empty;
                    string DataType = string.Empty;
                    string PreFix = " public ";
                    string PostFix = " {{ get; set; }}";

                    string Main = string.Empty;

                    DataType = GetDataType(item.Data_Type);
                    Name = item.Column_Name;
                    Main = PreFix + DataType + " " + item.Column_Name + PostFix + "\n";
                    content.AppendFormat(Main, Environment.NewLine);

                }
                content.Append("  } \n");
                content.AppendFormat("}}\n", Environment.NewLine); //class close

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, strModelName + ".cs");

                SaveFile(path, content.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateDomainConstants(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                StringBuilder content = new StringBuilder();

                //write namespace
                content.AppendFormat("using System;\n", Environment.NewLine);

                //wrapper 
                content.AppendFormat("namespace " + txtDataConstNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.Append("  public class " + strModelName + "Infra : BaseInfra \n  { \n");
                foreach (TableDesign item in lst)
                {
                    //write constant
                    content.Append(" public const string " + item.Column_Name.ToUpper() + " =\"" + item.Column_Name + "\";\n");
                }

                content.Append("  } \n"); //class close


                content.AppendFormat("}}\n", Environment.NewLine); //namespace close

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, strModelName + "Infra.cs");

                SaveFile(path, content.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateDomainInterfaces(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                string interfaceName = "I" + strModelName + "Repository";
                StringBuilder content = new StringBuilder();
                //write namespace
                content.AppendFormat("using System;\n", Environment.NewLine);
                //wrapper 
                content.AppendFormat("namespace " + txtDomainInterfacesNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.Append("  public interface " + interfaceName + " : IRepository<" + strModelName + "> \n  { \n");
                content.Append("  }} \n"); //class close

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, interfaceName + ".cs");

                SaveFile(path, content.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateRepositoryClasses(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                string className = strModelName + "Repository";

                StringBuilder content = new StringBuilder();
                content.AppendFormat("using System;\n", Environment.NewLine);
                content.AppendFormat("using " + txtDataConstNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using " + txtDapperNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using " + txtDomainNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using " + txtDomainInterfacesNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using Microsoft.Extensions.Options;\n", Environment.NewLine);

                content.AppendFormat("namespace " + txtRepoClassNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);

                content.Append("  public class " + className + " : Repository<" + strModelName + ">, I" + strModelName + "Repository \n  { \n");

                content.AppendFormat(" public " + className + "(IOptions<ReadConfig> connStr, IDapperResolver<" + strModelName + "> resolver) : base(connStr, resolver) \n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat(" }} \n", Environment.NewLine);

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, className + ".cs");

                SaveFile(path, content.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateServiceInterfaces(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                string interfaceName = "I" + strModelName;

                StringBuilder content = new StringBuilder();
                content.AppendFormat("using System;\n", Environment.NewLine);
                content.AppendFormat("using " + txtDomainNamespace.Text + ";\n", Environment.NewLine);

                content.AppendFormat("namespace " + txtServiceInterfaceNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);

                content.AppendFormat("public interface " + interfaceName + " : IServiceBase<" + strModelName + ">");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat(" }} \n", Environment.NewLine);

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, interfaceName + ".cs");

                SaveFile(path, content.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateServiceClasses(List<TableDesign> lst, string strModelName, string strFilePath)
        {
            try
            {
                string className = strModelName + "Service";
                string repoObjName = "i" + strModelName + "Repository";

                StringBuilder content = new StringBuilder();
                content.AppendFormat("using System;\n", Environment.NewLine);
                content.AppendFormat("using System.Collections.Generic;\n", Environment.NewLine);
                content.AppendFormat("using System.Threading.Tasks;\n", Environment.NewLine);
                content.AppendFormat("using " + txtDomainNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using " + txtDomainInterfacesNamespace.Text + ";\n", Environment.NewLine);
                content.AppendFormat("using " + txtServiceInterfaceNamespace.Text + ";\n", Environment.NewLine);

                content.AppendFormat("namespace " + txtServiceClassNamespace.Text + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);

                content.AppendFormat("public class " + className + " : I" + strModelName + "\n", Environment.NewLine);
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("private I" + strModelName + "Repository " + repoObjName + "; \n", Environment.NewLine);
                content.AppendFormat("public " + className + "(I" + strModelName + "Repository I" + strModelName + "Repository) : base()");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("" + repoObjName + " = I" + strModelName + "Repository;");
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat("public Task<int> AddEditAsync(" + strModelName + " obj)");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("throw new NotImplementedException();");
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat("public Task<int> DeleteAsync(" + strModelName + " obj)");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("throw new NotImplementedException();");
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat("public Task<IEnumerable<" + strModelName + ">> GetAllAsync(" + strModelName + " obj)");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("throw new NotImplementedException();");
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat("public Task<" + strModelName + "> GetAsync(" + strModelName + " obj)");
                content.AppendFormat(" {{ \n", Environment.NewLine);
                content.AppendFormat("throw new NotImplementedException();");
                content.AppendFormat(" }} \n", Environment.NewLine);

                content.AppendFormat(" }} \n", Environment.NewLine);
                content.AppendFormat(" }} \n", Environment.NewLine);

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                string path = Path.Combine(strFilePath, className + ".cs");

                SaveFile(path, content.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDI(string strModelName)
        {
            try
            {
                string res = "services.AddSingleton<I" + strModelName + ", " + strModelName + "Service>();" + Environment.NewLine;
                res += "services.AddSingleton<I" + strModelName + "Repository, " + strModelName + "Repository>();" + Environment.NewLine;

                txtNInjectMapping.Text = res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        #region Get Table Design
        private DataSet GetDataSet(string strTable)
        {
            DataSet ds = null;
            string _strConnString = string.Empty;
            SqlCommand _sqlcmd = new SqlCommand();
            SqlParameter _sqlpara;

            SQLObject _sqlObject = new SQLObject();

            _sqlcmd.CommandText = "sproc_GetTableDesign";


            _sqlpara = new SqlParameter("@TableName", strTable);
            _sqlcmd.Parameters.Add(_sqlpara);

            try
            {
                _strConnString = _sqlObject.GetMasterDBConnString();
                ds = _sqlObject.SqlDataAdapter(_sqlcmd, _strConnString);
            }
            catch (Exception expCommon)
            {
                throw expCommon;
            }
            return ds;
        }
        private string GetDataType(string DataType)
        {
            string ret = string.Empty;

            switch (DataType)
            {
                case "varchar":
                    ret = "string";
                    break;
                case "nvarchar":
                    ret = "string";
                    break;
                case "bit":
                    ret = "bool";
                    break;
                case "datetime":
                    ret = "DateTime";
                    break;
                case "date":
                    ret = "DateTime";
                    break;
                case "int":
                    ret = "int";
                    break;
                default:
                    ret = "";
                    break;
            }
            return ret;
        }
        #endregion
    }
}

public class TableDesign
{
    public string Column_Name { get; set; }
    public string Data_Type { get; set; }
    public int Character_Maximum_Length { get; set; }
}
