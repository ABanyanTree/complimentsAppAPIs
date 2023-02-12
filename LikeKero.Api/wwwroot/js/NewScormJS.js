   //top.frames.frmPoller.document.location.href = 'SessionPoller.aspx';

    function fGoHome()
    {
        if (gNewWin)
        {
            if(gNewWin.close)
            {
                gNewWin.close();
            }
        }
        //Do the course exit functionality here
		if (top.frames.ContentFrame.document.layers)
		{ // Netscape 4.0+
					var surl = top.frames.ContentFrame.document.location.href;
					var shttp = "";
					var stemp = "";
					var ind = surl.indexOf("//");

					shttp = surl.substring(0,ind+2);

					var arr = surl.split("//");
					stemp =  arr[1];
					arr = stemp.split("/");
					surl = arr[0] + "/" + arr[1];
					surl = shttp + surl;
					top.frames.ContentFrame.document.location.href = surl + "/" + "BVOnCourseExit.aspx?GoToUrl=Package";
		}
		else
		{
			top.frames.ContentFrame.document.location.href = "BVOnCourseExit.aspx?GoToUrl=Package";
		}
		 top.opener.document.location.href = top.opener.document.location.href;
    }

    
    var arrSCO = new Array();            
    var arrManifest;
    var arrManifestNodes = new Array();
    var gCoursePath;
    var API = APIAdapter;
    var gContentPath;         
    var gTimeInterval = "";
    var gLearnerId = "";
    var gStudentId = "";
    var gPageSettings = "";
    var gTrackScoreSettingFromLMS = "";
    var LaunchParameter = "";
    var sLearnerLanguageId = "";
    var gManifestId = "";
    var gBookmark = "";
    var gUserDataURL = "";

    var arrFavorites = new Array();

    function ManifestNode(lArr)
    {
        this.identifier = lArr[0];
        this.title = lArr[1];
        this.winTarget = lArr[2];
        this.prerequisites = lArr[3];
        this.datafromlms = lArr[4];
        this.masteryscore = lArr[5];
        this.maxtimeallowed = lArr[6];
        this.timelimitaction = lArr[7];
        this.identifierref = lArr[8];
        this.scormtype = lArr[9];
        this.type = lArr[10];
        this.base = lArr[11];
        this.href = lArr[12];
    }

	function CSCO(lArr)
	{
		if(lArr)
		{
			this.identifier = lArr[0];
			this.cmi__core__student_id = lArr[1];
			this.cmi__core__student_name = lArr[2];
			this.cmi__core__lesson_location = lArr[3];
			this.cmi__core__credit = lArr[4];
			this.cmi__core__lesson_status = lArr[5];
			this.cmi__core__entry = lArr[6];
			this.cmi__core__score__raw = lArr[7];
			this.cmi__core__score__max = lArr[8];
			this.cmi__core__score__min = lArr[9];
			this.cmi__core__total_time = lArr[10];
			this.cmi__core__lesson_mode = lArr[11];
			this.cmi__core__exit = lArr[12];
			this.cmi__core__session_time = lArr[13];
			
			this.cmi__suspend_data = lArr[14];
			this.cmi__launch_data = lArr[15];
			this.cmi__comments = lArr[16];
			this.cmi__comments_from_lms = lArr[17];
			this.cmi__objectives___count = lArr[18];
			
			
			var a2 = new Array();
			a2=lArr[19];
			var a2 = new Array();
			
			
			a2 = lArr[19].split("#&bv&#");
			for (var i=0;i<a2.length;i++){
			    var a3 = new Array();
			    a3 = a2[i].split(",");
			    
			    eval("this.cmi__objectives___" + i + "__id = a3[0]");
				eval("this.cmi__objectives___" + i + "__score__raw = a3[1]");
				eval("this.cmi__objectives___" + i + "__score__max = a3[2]");
				eval("this.cmi__objectives___" + i + "__score__min = a3[3]");
				eval("this.cmi__objectives___" + i + "__status = a3[4]");
			}
			
//			for(var i=0;i<a2.length;i++)
//			{
//			
////			    this.cmi__objectives___" + i + "__id = lArr[19][0];
////			    this.cmi__objectives___" + i + "__score__raw = lArr[19][1];
////			    this.cmi__objectives___" + i + "__score__max = lArr[19][2];
////			    this.cmi__objectives___" + i + "__score__min = lArr[19][3];
////			    this.cmi__objectives___" + i + "__status = lArr[19][4];
//			    
//				eval("this.cmi__objectives___" + i + "__id = lArr[19][0]");
//				eval("this.cmi__objectives___" + i + "__score__raw = lArr[19][1]");
//				eval("this.cmi__objectives___" + i + "__score__max = lArr[19][2]");
//				eval("this.cmi__objectives___" + i + "__score__min = lArr[19][3]");
//				eval("this.cmi__objectives___" + i + "__status = lArr[19][4]");
//			}
			this.cmi__student_data__mastery_score = lArr[20];
			this.cmi__student_data__max_time_allowed = lArr[21];
			this.cmi__student_data__time_limit_action = lArr[22];
			this.cmi__student_preference__language=lArr[23];
		}
		else
		{
			this.identifier;
			this.cmi__core__student_id="";
			this.cmi__core__student_name="";
			this.cmi__core__lesson_location="";
			this.cmi__core__credit = "credit";
			this.cmi__core__lesson_status = "not attempted";
			this.cmi__core__entry = "ab-initio";
			this.cmi__core__score__raw="";
			this.cmi__core__score__max="";
			this.cmi__core__score__min="";
			this.cmi__core__total_time = "0000:00:00.00";
			this.cmi__core__lesson_mode = "normal";
			this.cmi__core__exit="";
			this.cmi__core__session_time="0000:00:00.00";
			this.cmi__suspend_data="";
			this.cmi__launch_data="";
			this.cmi__comments="";
			this.cmi__comments_from_lms="";
			this.cmi__objectives___count="0";
			this.cmi__student_data__mastery_score="";
			this.cmi__student_data__max_time_allowed="";
			this.cmi__student_data__time_limit_action="";
			this.cmi__student_preference__language=sLearnerLanguageId;
		}
	}

	var arrParams ; 
    arrSCO[arrSCO.length] = new CSCO(arrParams);       
    var gContentModuleName;           

    var gCoursesPath ;
    var gManifestPath ;
    var gLearnerName ;
    var gAppURL ;
    //var gBookmark = "";	
	var gPageId = 'LtCoursePlayer';
	var gNewWin;
    var gURL;
    
    //var gPageSettings="width=800,height=610,top=0,left=0,toolbars=no, resizable=no";
    
    function fOpenScoAsset(lUrl,lNodeId,lTypeOfNode)
    {
        
        fSetNode("Identifier", lNodeId);

        if(lTypeOfNode == "asset")
            fDoForAsset(lNodeId);
        else
            fSetNode("Status", "NotInitialized");

        gURL = lUrl;
        
        
        //parent.ContentSrvFrame.location.href = "LMSNewScormDisplaySCO.htm"; //check this later
        //sendDataToServer();


        //alert(parent.ContentSrvFrame.location.href);
		
		//if(!IsSingleLaunchSco())
        //{
        //    //parent.document.getElementById("frmsetLaunch").rows="0,0,0,*";
        //}
        //else
        //{
        //    if (window.parent.name=="course")
    	//    {
        //        parent.document.getElementById("frmsetLaunch").rows="0,0,0,*";
        //    }
        //    else
        //    {
        //        parent.document.getElementById("frmsetLaunch").rows="50,0,0,*";
        //    }
        //}

		
    }

	function fDoForAsset(lNodeId)
	{
		fCheckAndCreateSCOBlockForAsset();
	}
	
/*    if(gBookmark != "")
    {
        fGoToBookmark();
    }*/

    function fMoveNext()
    {
        return fMovePreviousNext('Next');
    }

    function fMovePrevious()
    {
        return fMovePreviousNext('Previous');
    }

    function fMovePreviousNext(lCase)
    {
        var lIdentifier = fGetNode("Identifier");
        var oNode = fGetNextPrevItemNodeObject(lIdentifier, lCase);
        if (oNode)
        {
            var lUrl =  fGetValue(oNode, "href");
            lIdentifier =  fGetValue(oNode, "identifier");
            fSetNextPreviousScoAsset(gContentPath+lUrl,lIdentifier);

            return true;
        }

        return false;
    }

    function fChangeOverForNext()
    {          
        var lIdentifier = fGetNode("Identifier");
        var oNode = fGetNextPrevItemNodeObject(lIdentifier, 'Next');

        if (oNode)
        {
            //fChangeContentFrameURL(gAppURL+"/ScormLibrary/ChangeOverForNext.html");            
            gURL = gAppURL+"ScormLibrary/ChangeOverForNext.html";            
            return true;
        }

        return false;
    }

    function fChangeOverForPrevious()
    {                
        var lIdentifier = fGetNode("Identifier");
        var oNode = fGetNextPrevItemNodeObject(lIdentifier, 'Previous');

        if (oNode)
        {
            //fChangeContentFrameURL(gAppURL+"/ScormLibrary/ChangeOverForPrevious.html");
            gURL = gAppURL+"ScormLibrary/ChangeOverForPrevious.html";
            return true;
        }

        return false;
    }

    function fSetNextPreviousScoAsset(lUrl,lNodeId)
    {
        fSetNode("Identifier", lNodeId);
        fSetNode("Status", "NotInitialized");
        gURL = lUrl;
    }




