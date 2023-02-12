//--------------------------------------------------------------------------------
//GLOBAL VARIABLES	
//--------------------------------------------------------------------------------

	//var arrSCO = top.arrSCO;
	var arrFavorites = top.arrFavorites;
	var gCourseLoaded = false;
	var gTotalManifestNodes;
	var bExited = false;

//---------------------------------------------------------------------------------


	function fResetError(){
		fSetError("0");
	}

	function fSetError(lErrNo){
		gArrCurrentStatus["LastError"] =  lErrNo;
	}

	function fCheckForEmptyString(lString){
		if (lString != ""){
			fSetError("201");		//Invalid argument error
			return false;
		}
		
		return true;
	}

	function fCheckErrorStatus(){
		if (fGetNode("LastError") == "0"){
			return "true";
		}
		else{
			return "false";
		}
	}
	
	function fIsFunctionAllowed(lFunctionName){
		var sRetVal = gArrCurrentStatus["Status"];
		var sFunctions = gArrFunctionsState[sRetVal];
		if(sFunctions)
		if (sFunctions.indexOf(lFunctionName) != -1){
			return true;
		}

		if (sRetVal == "NotInitialized"){
			fSetError("301");
		}
		else{
			fSetError("101");
		}
		
		return false;
	}
	
	//This function will be called from LMSInitialize to set the core.entry
	//depending on core.exit
	function fModifyCoreEntry(){
		var sExitVal = fDirectGetDataFormUserDataXML("cmi__core__exit");
		
		if (sExitVal == "suspend"){
			fDirectSetDataToUserDataXML("cmi__core__entry", "suspend");
		}
		else{
			fDirectSetDataToUserDataXML("cmi__core__entry", "");
		}
	}


	function LMSIntInitialize(lParam){
//alert("LMSIntInitialize");
		if (!fCheckForEmptyString(lParam)){
			return "false";
		}

		if (!fIsFunctionAllowed("LMSInitialize")){
			return "false";
		}


		fResetError();
		fModifyCoreEntry();
		fCheckAndCreateSCOBlock();
		fSetNode("Status", "Initialized");

		/*Call ObjCurrentStatus.fGetNode("Root/Identifier", lCurrentNodeId)
		Call ObjCurrentStatus.fGetNode("Root/InitializedNodeId", sInitNodeId)
		Call ObjCurrentStatus.fSetNode("Root/InitializedNodeId", lCurrentNodeId)
		Call ObjCurrentStatus.fSetNode("Root/LastInitializedNodeId", sInitNodeId)*/
		
		return fCheckErrorStatus();
	}

	function LMSIntGetValue(lParam){
//alert("LMSIntGetValue");	
		/*if((lParam.length > 255)){
			//vinod bonde 
			alert("Blank")
			return;
		}*/
		if (!fIsFunctionAllowed("LMSGetValue")){
			return "";
		}

		fResetError();
		
		return fGetSetUserData(lParam, "", "Get");
		//return fGetUserData(lParam);
	}
	
	function LMSIntSetValue(lParam, lData){
	//debugger 
//alert("LMSIntSetValue");
		//LMSIntSetValue = "false"
		
		//Modified the API by Rohit on 30th Oct 2009 for SAI Global LMS
		//Below condition is an excpetion to handle Tracking Score based on the LMS level settings
		if (lParam == "cmi.core.score" && gTrackScoreSettingFromLMS == "false"){
		    return "true";
		}

		//Modified the API by Rohit on 30th Oct 2009 for SAI Global LMS
		//Below condition is an excpetion to handle Tracking Response based on the LMS level settings
		if (lParam == "cmi.interactions" && gTrackResponseSettingFromLMS == "false"){
		    return "true";
		}
		
		if (!fIsFunctionAllowed("LMSSetValue")){
			return "false";
		}

		if (lParam == "cmi.core.lesson_status" && lData == "not attempted"){
			fSetError("405");
			return "false";
		}
		
		//uncommented by rohit----------
		/*
		if (lParam == "cmi.objectives.number.id" && (lData == '')){
			fSetError("405");
			return "false";
		}
		if (lParam == "cmi.interactions.number.id" && (lData == '')){
			fSetError("405");
			return "false";
		}

		if (lParam == "cmi.interactions.number.objectives.number.id" && (lData == '')){
			fSetError("405");
			return "false";
		}
		if((lData.length > 255) && (lParam != "cmi.suspend_data") && (lParam != "cmi.comments")){
			fSetError("405");
			return "false";
		}*/
		//---------------------

		fResetError();
		
		//fSetUserData(lParam, lData);
		fGetSetUserData(lParam, lData, "Set");
		
		return fCheckErrorStatus()
	}

	function LMSIntFinish(lParam){
		//LMSIntFinish = "false"
//alert("LMSIntFinish");
		if (!fCheckForEmptyString(lParam)){
			return "false";
		}

		if (!fIsFunctionAllowed("LMSFinish")){
			return "false";
		}

		fModifySCOParamsBeforeLMSFinish();
		fResetError();
		LMSIntCommit("");
		fSetNode("Status", "Finished");

		var LMSIntFinish = fCheckErrorStatus();
		fSetLMSFinishReturn(LMSIntFinish);
		return LMSIntFinish;
	}
	
	function LMSIntCommit(lParam){
//alert("LMSIntCommit");
		//LMSIntCommit = "false"

		if (!fCheckForEmptyString(lParam)){
			return "false";
		}

		if (!fIsFunctionAllowed("LMSCommit")){
			return "false";
		}

		fResetError();

		if (!fSendRequest("SaveCurrentSco")){
			fSetError ("101");
		}

		return fCheckErrorStatus();
	}

	function LMSIntGetLastError(){
//alert("LMSIntGetLastError");
		//LMSIntGetLastError = "true"
		
		if (!fValidateFinishedState("LMSGetLastError")){
			return "false";
		}

		return fGetNode("LastError");
	}

	function LMSIntGetErrorString(lErrNo){
//alert("LMSIntGetErrorString");
		//LMSIntGetErrorString = "true"
		
		if (!fValidateFinishedState("LMSGetErrorString")){
			return "false";
		}

		return fGetErrorString(lErrNo);
	}

	function LMSIntGetDiagnostic(lErrNo){
		//LMSIntGetDiagnostic = "true"

		if (!fValidateFinishedState("LMSGetDiagnostic")){
			return "false";
		}

		return fGetErrorDiagnostic(lErrNo);
	}

//---------------------------------------------------------------------------

	function fGetErrorString(lErrNo){
		if (gErrorLookup[lErrNo]){
			return gErrorLookup[lErrNo][0];
		}
		
		return "";
	}

	function fGetErrorDiagnostic(lErrNo){
		if (gErrorLookup[lErrNo]){
			return gErrorLookup[lErrNo][1];
		}
		
		return "";
	}
	
	function fValidateFinishedState(lFunctionName){
		if (!fIsFunctionAllowed(lFunctionName)){
			return false;
		}

		if (fGetNode("Status") == "Finished"){
			if (fGetLMSFinishReturn() == "true"){
				return false;
			}
		}

		return true;
	}
	
	function fGetLMSFinishReturn(){
		return gArrFunctionsState["LMSFinishReturn"];
	}


	function fSetLMSFinishReturn(lValue){
		gArrFunctionsState["LMSFinishReturn"] = lValue;
	}

	function fCheckAndCreateSCOBlockForAsset(){
		var sIdentifier = fGetNode("Identifier");
		var oSCO = fGetCurrentSCO();

		if (!oSCO){
			oSCO = new CSCO();
			oSCO.identifier = sIdentifier;
         
			//set Learner id
			oSCO.cmi__core__student_id = gStudentId;

			//set Learner Name
			oSCO.cmi__core__student_name = gLearnerName;
            oSCO.cmi__student_preference__language = sLearnerLanguageId;

				//set lesson_status to completed vinod bonde
				//if(!gIsQuiz){
				 oSCO.cmi__core__lesson_status = "completed";
	         //}

         
			arrSCO[arrSCO.length] = oSCO;

			fSendRequest("SaveCurrentSco");
      }
	}

	function fCheckAndCreateSCOBlock(){
	
		var sIdentifier = fGetNode("Identifier");
		var oSCO = fGetCurrentSCO();

		if (!oSCO){
			oSCO = new CSCO();
			oSCO.identifier = sIdentifier;

			//set Learner id
			oSCO.cmi__core__student_id = gStudentId;

			//set Learner Name
			oSCO.cmi__core__student_name = gLearnerName;
			oSCO.cmi__student_preference__language = sLearnerLanguageId;
			
			arrSCO[arrSCO.length] = oSCO;
			
			//set data from Manifest
			//fSetDataFromManifest("cmi__launch_data", "adlcp:datafromlms");
			fSetDataFromManifest("cmi__launch_data", "datafromlms");//modified 
			
	//fSetDataFromManifest("cmi__student_data__mastery_score", "adlcp:masteryscore");//commented by ROhit Gajarmal code not getting MasterScore 
			fSetDataFromManifest("cmi__student_data__mastery_score", "masteryscore"); // modified by Rohit(SAI Global)
		//	fSetDataFromManifest("cmi__student_preference__language", "language");

			fSetDataFromManifest("cmi__student_data__max_time_allowed", "adlcp:maxtimeallowed");
			fSetDataFromManifest("cmi__student_data__time_limit_action", "adlcp:timelimitaction");
		}
	}

	function fCheckAndCreateSCOBlockAndSetCompleted(lNodeId){
		var sIdentifier = lNodeId
		var oSCO = fGetSCO(lNodeId);

		if (!oSCO){
			oSCO = new CSCO();
			oSCO.identifier = sIdentifier;

			//set Learner id
			oSCO.cmi__core__student_id = gStudentId;

			//set Learner Name
			oSCO.cmi__core__student_name = gLearnerName;
			oSCO.cmi__student_preference__language = sLearnerLanguageId;
			
			oSCO.cmi__core__lesson_status = "completed";
			arrSCO[arrSCO.length] = oSCO;
			
			//set data from Manifest
			fSetDataFromManifest("cmi__launch_data", "adlcp:datafromlms");
			fSetDataFromManifest("cmi__student_data__mastery_score", "adlcp:masteryscore");
			fSetDataFromManifest("cmi__student_data__max_time_allowed", "adlcp:maxtimeallowed");
			fSetDataFromManifest("cmi__student_data__time_limit_action", "adlcp:timelimitaction");
			
		}
	}

	function fSetDataFromManifest(lXPath, lParam){
	
		retVal = fGetDataFromManifest(lParam);
		if(lXPath == "cmi__student_data__mastery_score"){
	        //alert(lXPath +"\n\n"+lParam +"\n\n"+retVal);
	    }
	
		if (retVal != ""){
			fDirectSetDataToUserDataXML(lXPath, retVal);
		}
	}

	/*
	function fGetUserData(lParam, lData){
		var lData = ""
		return fGetSetUserData(lParam, lData, "Get");
	}
	*/
	
	function fIsNumeric(lValue){
		if ((/^[0-9 ]+$/.test(lValue))){
			return true;
		}

		return false;
	}

	
	function fGetSetUserData(lParam, lData, lGetSet){
		
		//fGetSetUserData = "false"
		
		//var bSave = false;

		if (lParam.indexOf(".") == -1){
			fSetError("201");
			return "";
		}

		/*
			- Clonning User data xml to achieve the functionality to roll back
			  the changes if required
			- Make any changes to this clone node and if no error ocurred then 
			  copy it back to the user data xml

			set xmlUserDataClone = CreateObject("Microsoft.xmldom")
			xmlUserDataClone.async = false
			call xmlUserDataClone.loadXML(xmlUserData.xml)
		*/

		var arrParams = lParam.split(".");
		var sIdentifier = fGetNode("Identifier");
		
		//gCurrentFunctionParams = "Root/sco[@identifier='" & lIdentifier & "']"
		//userData_RunningPath = gCurrentFunctionParams      	//to check against userdata xml

		var userData_RunningPath = "";
		var sPreviousObjectivePath = "";              	//to check against userdata xml for objective sequence
		var template_RunningPath = "";                	//to check against template xml
		var currentNodeName = "";
		var sParamForObjective = "";
		var bObjective = false;

		for (var i=0;i<arrParams.length;i++){
			if (fIsNumeric(arrParams[i])){
				sPreviousObjectivePath = userData_RunningPath.substr(2) + "__" + "_" + (arrParams[i] - 1);
				arrParams[i] = "_" + arrParams[i];
				currentNodeName = "_number";             //to check against template xml
				bObjective = true;
			}
			else{
				//sParamForObjective = arrParams[i];
				currentNodeName = arrParams[i];
				if (fIsNumeric(currentNodeName.substr(0, 1))){
					fSetError("201");
					return "";
				}
			}
			
			template_RunningPath = template_RunningPath + "__" + currentNodeName;     //to check against template xml

			/*
				if lGetSet = "Set" then
					call fCreateOrReplaceNode(userData_RunningPath, arrParams(i))
				end if
			*/
			
			userData_RunningPath = userData_RunningPath + "__" + arrParams[i];  			//to check against userdata xml
		}

		template_RunningPath = template_RunningPath.substr(2);
		userData_RunningPath = userData_RunningPath.substr(2);

		if (lGetSet == "Set"){
			fSetNodeValue(template_RunningPath, userData_RunningPath, lData, bObjective, sPreviousObjectivePath);
		}
		else{
			return fGetNodeValue(template_RunningPath, userData_RunningPath);
		}

		var bERROR = fGetNode("LastError");
		if (bERROR != "0"){
			return "";
		}

		/*
			if lGetSet = "Set" Or bSave then
				xmlUserData.async = false
				call xmlUserData.loadXML(xmlUserDataClone.xml)
			end if

			set xmlUserDataClone = nothing
		*/
		
		return "";
	}
	
	function fGetLastNodeInContext(lXPath){
		ArrNodes = lXPath.split("__");
		return ArrNodes[ArrNodes.length-1];
	}
	
	//verifies the context passed to the function against the template
	function fIsCorrectContext(lContext){
		eval("var oTemplateNode = gMasterTemplate." + lContext);
		return (!fIsUndefined(oTemplateNode));
		//return (typeof(oTemplateNode) != 'undefined');
	}
	
	function fIsUndefined(val){
		return typeof(val) == 'undefined';
	}
	
	//verifies the context passed to the function is implemented or not
	function fIsImplemented(lContext){
		var sRetVal = fGetValueFromMasterTemplate(lContext + ".Implemented");
		return (sRetVal != "false");
	}
	
	function fCanWrite(lNode){
		var sRetVal = lNode.Mode;

		if (sRetVal == "Write" || sRetVal == "Both"){
			return true;
		}

		return false;
	}

	function fCanRead(lNode){
		var sRetVal = lNode.Mode;
		if (sRetVal == "Read" || sRetVal == "Both"){
			return true;
		}

		return false;
	}

	
	function fCheckError(lCase, lContext){
		eval("var oTemplateNode = gMasterTemplate." + lContext);
		var sErrorNumber = "0";
		var sLastNode = fGetLastNodeInContext(lContext);

		switch(lCase){
			case "201":											//correct Context
				if (!fIsCorrectContext(lContext)){
					sErrorNumber = "201";

					if (sLastNode == "_children"){
						sErrorNumber = "202";
					}

					if (sLastNode == "_count"){
						sErrorNumber = "203";
					}
				}
				break;

			case "202":
				//if sLastNode = "_children" then
				//   sErrorNumber = "202"
				//end if
				break;

			case "203":
				if (sLastNode == "_count"){
					sErrorNumber = "203";
				}
				break;

			case "401":           							//IsImplemented
				if (!fIsImplemented(lContext)){
					sErrorNumber = "401";
				}
				break;

			case "404":           							//Can Read
				if (!fCanRead(oTemplateNode)){
					sErrorNumber = "404";
				}
				break;

			case "402":           
				if (sLastNode == "_children" || sLastNode == "_count" || sLastNode == "_version"){
					sErrorNumber = "402";
				}
				break;

			case "403":           
				if (!fCanWrite(oTemplateNode)){
					sErrorNumber = "403";
				}
				break;
		}

		if (sErrorNumber == "0"){
			return false;
		}
		else{
			fSetError(sErrorNumber);
			return true;
		}
	}

	
	function fGetNodeValue(lTemplateNodePath, lUserNodePath){
		//fGetNodeValue = "false"
		
		//correct Context
		if (fCheckError("201", lTemplateNodePath)){
			return "";
		}
			
		//IsImplemented
		if (fCheckError("401", lTemplateNodePath)){
			return "";
		}

		//Can Read
		if (fCheckError("404", lTemplateNodePath)){
			return "";
		}

		var sRetVal = "";
		
		if (fGetLastNodeInContext(lTemplateNodePath) == "_children"){
			//if not XMLLib.fgetvalue(xmlTemplateDoc, lTemplateNodePath, lGetValue) then
			sRetVal = fGetValueFromMasterTemplate(lTemplateNodePath + ".Text");
			
			if (fIsUndefined(sRetVal)){
				fSetError("401");
			}
		}
		else{
			if (fGetLastNodeInContext(lTemplateNodePath) == "_count"){
				//inscrease the ._count
				fUpdateCount(lUserNodePath);
				
				//call XMLLib.fgetvalue(xmlUserDataClone, lUserNodePath, lGetValue)
				sRetVal = fDirectGetDataFormUserDataXML(lUserNodePath);
				if (sRetVal == ""){
					sRetVal = "0";
				}
			}
			else{
				sRetVal = fDirectGetDataFormUserDataXML(lUserNodePath);
				if (sRetVal == ""){
					fSetError("201");
				}
			}
		}

		fSetError("0");
		return sRetVal;
	}
	
	function fSetNodeValue(lTemplateNodePath, lUserNodePath, lSetValue, lbObjective, lsPreviousObjectivePath){
		//fSetNodeValue = "false"
		
		//incorrect Context
		if (fCheckError("201", lTemplateNodePath)){
			return false;
		}
		
		//not Implemented
		if (fCheckError("401", lTemplateNodePath)){
			return false;
		}

		//reserved keywords
		if (fCheckError("402", lTemplateNodePath)){
			return false;
		}

		//Can Write
		if (fCheckError("403", lTemplateNodePath)){
			return false;
		}

		//check sequence for objectives, interactions and interactions.objectives
		if (!fCheckForObjectiveSequence(lsPreviousObjectivePath, lbObjective)){
			return false;
		}

		if (!fValidateSetValue(lTemplateNodePath, lSetValue)){
			return false;
		}

		//call XMLLib.fCreateFirstContext(xmlUserDataClone, lUserNodePath, oNode)
		//append to the previously set comments
		if (lTemplateNodePath == "cmi__comments"){
			lSetValue = fDirectGetDataFormUserDataXML(lUserNodePath) + lSetValue;
		}

		//call XMLLib.fAddCdataToNode(xmlUserDataClone, oNode, lSetValue)
		fDirectSetDataToUserDataXML(lUserNodePath, lSetValue);

		//inscrease the ._count if data for new objective is added to the userdata
		//call fUpdateCount(lTemplateNodePath)

		fSetError("0");
		return true;
	}

	function fCheckForObjectiveSequence(lPreviousObjectivePath, lbObjective){
		//fCheckForObjectiveSequence = true
		
		if (!lbObjective){
			return true;
		}
		
		if (lPreviousObjectivePath.indexOf("_-1") != -1){
			return true;
		}

		sRetVal = fDirectGetDataFormUserDataXML(lPreviousObjectivePath + "__id");
		if (sRetVal == ""){
			fSetError("201");
			return false;
		}
		
		return true;
	}

   function fValidateSetValue(lContext, lVal){
      //fValidateSetValue = "false"
      
      if (!fSpChecks(lContext, lVal)){
         return false;
      }
      
      var sRetVal = fGetValueFromMasterTemplate(lContext + ".ValidationType");
      var sLookupName = "";

      if (sRetVal.toUpperCase() == "CMIVOCABULARY"){
      	sLookupName = fGetValueFromMasterTemplate(lContext + ".LookupName");
      }

      //lVal = fGetFirstCharForValidation(lstNodes(0), lVal)
      
      if (!fExtResValidate(sRetVal, lVal, sLookupName, fGetValueFromMasterTemplate(lContext + ".LValue"), fGetValueFromMasterTemplate(lContext + ".HValue"))){
         fSetError("405", "true");
         return false;
      }

		/*
			if ucase(retVal) = "CMIVOCABULARY" then
				lVal = retTempVal			'variable from javascript
			end if
		*/
		
      return true;
   }
   
	function fSpChecks(lContext, lVal){
		//fSpChecks = true

		switch (lContext){
			case "cmi__core__score__raw":
			case "cmi__core__score__max":
			case "cmi__core__score__min":
			case "cmi__objectives___number__score__raw":
			case "cmi__objectives___number__score__max":
			case "cmi__objectives___number__score__min":
				if (lVal == ""){
					return true;
				}
				if (fExtResValidate("CMIDECIMAL", lVal, "", "", "")){
					//value must be 0 to 100
					if (parseFloat(lVal) < 0 || parseFloat(lVal) > 100){
						fSetError("405", "true");
						return false;
					}
				}
			break;
		}
		
		return true;
	}
	
	function fUpdateCount(lUserNodePath){
		//cmi__objectives___count										n.id
		//cmi__interactions___count									n.id
		//cmi__interactions__n__objectives___count				n.id
		//cmi__interactions__n__correct_responses___count		n.pattern

		var sPreText = lUserNodePath.substr(0, lUserNodePath.indexOf("___count"));
		var sPostText
		if (lUserNodePath.indexOf("correct_responses") != -1){
			sPostText = "pattern";
		}
		else{
			sPostText = "id";
		}

		var i = -1;
		for(;;){
			if (!fNodeExistsInCurrentSco(sPreText + "___" + (++i) + "__" + sPostText)){
				break;
			}
		}

		fDirectSetDataToUserDataXML(lUserNodePath, i.toString());
		//bSave = true;
	}

	function fModifySCOParamsBeforeLMSFinish(){
		fModifyLessonStatus();
		fModifyTotalTime();
	}

	//This function will be called before LMSFinish to add session_time
	//to total_time
	function fModifyTotalTime(){
		var sTotalTime = fDirectGetDataFormUserDataXML("cmi__core__total_time");
		var sSessionTime = fDirectGetDataFormUserDataXML("cmi__core__session_time");
		
		sTotalTime = fAddTime(sSessionTime, sTotalTime);
		if (sTotalTime != "false"){
			fDirectSetDataToUserDataXML("cmi__core__total_time", sTotalTime);
		}
	}

	//This function will be called before LMSFinish to correct the lesson_status
	//depending on mastery-score and raw-score if the test is for credit
	function fModifyLessonStatus(){
		var sLessonStatus = fDirectGetDataFormUserDataXML("cmi__core__lesson_status");
		
		if (sLessonStatus == "not attempted"){
			sLessonStatus = "completed";
			fDirectSetDataToUserDataXML("cmi__core__lesson_status", sLessonStatus);
		}

		if (sLessonStatus == "completed"){
			var sCredit = fDirectGetDataFormUserDataXML("cmi__core__credit");
			var sMasteryScore = fDirectGetDataFormUserDataXML("cmi__student_data__mastery_score", sMasteryScore);
			if (sCredit != "" && sMasteryScore != ""){
				var sRowScore = fDirectGetDataFormUserDataXML("cmi__core__score__raw");
				if (sRowScore != ""){
					if (parseFloat(sRowScore) < parseFloat(sMasteryScore)){
						fDirectSetDataToUserDataXML("cmi__core__lesson_status", "failed");
					}
					else{
						fDirectSetDataToUserDataXML("cmi__core__lesson_status", "passed");
					}
				}
			}
		}
	}

	function fAddTime(lTime1,lTime2){
		var arrTime1 = lTime1.split(":");
		var arrTime2 = lTime2.split(":");

		var extraMinute = 0;
		var extraHour = 0;
		
		if (arrTime1.length != 3 || arrTime2.length != 3){
			return "false";
		}
		
			var fSecond = parseFloat(arrTime1[2]) + parseFloat(arrTime2[2]);
			if (fSecond >= 60){
				fSecond = fSecond - 60;
				extraMinute = 1;
			}
			
			var fMinute = parseInt(arrTime1[1]) + parseInt(arrTime2[1]) + extraMinute;
			if (fMinute >= 60){
				fMinute = fMinute - 60;
				extraHour = 1;
			}

			var fHour = parseInt(arrTime1[0]) + parseInt(arrTime2[0]) + extraHour;

			//fSecond = Round(fSecond,2);
			
			var fSecondArr = fSecond.toString().split(".");
			
			if(fSecondArr.length == 1){
				if(fSecondArr[0].length == 1){
					fSecond = "0" + fSecondArr[0] + ".00";
				}
				else{
					fSecond = fSecondArr[0] + ".00"
				}
			}
			else{
				if(fSecondArr[0].length == 1){
					fSecondArr[0] = "0" + fSecondArr[0];
				}
				if(fSecondArr[1].length == 1){
					fSecondArr[1] = fSecondArr[1] + "0";
				}

				fSecond = fSecondArr[0] + "." + fSecondArr[1];
			}
			
			fMinute = fMinute.toString();
			if(fMinute.length == 1){
				fMinute = "0" + fMinute;
			}

			fHour = fHour.toString();
			if(fHour.length == 1){
				fHour = "0" + fHour;
			}
			
			
			if(fSecond.length > 5){
				fSecond = fSecond.substring(0,5);	
			}
			return fHour + ":" + fMinute + ":" + fSecond;
			
	}
	
//----------------------------------------------------------------------------

   function fSendRequest(lCase){      
	  var sURL = gUserDataURL;
      var sData;

      switch(lCase){
			case "Commit":
				var sData = fArrSCOToXML();
				sData = "<Root><Case>Commit</Case><StudentId>" + gStudentId + "</StudentId><ManifestId>" + gManifestId + "</ManifestId><UserDataXML>" + sData + "</UserDataXML></Root>";
				sURL = sURL  + "&Case=Commit"
			//	alert(lCase+"\n"+sData +"\n"+ sURL);
				fSendDataToLMSNEWServer(sData, sURL);
				break;
			case "Initialise":				
				break;
			case "SaveCurrentSco":
				sURL = sURL  + "&Case=SaveCurrentSco"				
				var sData = fSCOToXML(fGetNode("Identifier"));
				sData = "<Root><Case>SaveCurrentSco</Case><StudentId>" + gStudentId + "</StudentId><ManifestId>" + gManifestId + "</ManifestId><UserDataXML>" + sData + "</UserDataXML></Root>";
			//	alert(lCase+"\n"+sData +"\n"+ sURL);
				fSendDataToLMSNEWServer(sData, sURL);
				break;
			case "SaveFavorites":
				var sData = fFavoritesToXML();
				sData = "<Root><Case>SaveFavorites</Case><StudentId>" + gStudentId + "</StudentId><ManifestId>" + gManifestId + "</ManifestId><UserDataXML>" + sData + "</UserDataXML></Root>";
				sURL = sURL  + "&Case=SaveCurrentSco"
				
			//	alert(lCase+"\n"+sData +"\n"+ sURL);
				fSendDataToLMSNEWServer(sData, sURL);
				break;
      }

      return true;
   }

	var oNewWin;
	var gTimerId;
	function fSendDataToLMSNEWServer(lData, lURL)
	{
	    
	    sendDataToServer(lData);
	    //parent.ContentSrvFrame.controlFrame.document.formMain.TARecords.value = lData ;
	    //parent.ContentSrvFrame.controlFrame.document.formMain.method = "post";

		//parent.ContentSrvFrame.controlFrame.document.formMain.action=lURL ;
		//parent.ContentSrvFrame.controlFrame.document.formMain.submit();		
	}
	
	function fSendDataToServer(lData, lURL)
	{
	    alert("savedata::11");
		document.formMain.TARecords.value = lData;
	    document.formMain.target = "frmSubmitData";
		document.formMain.action = lURL;
         

		document.formMain.method = "post";
		document.formMain.submit();
		gTimerId = window.setInterval("fGetSCOResponse()", 2000);
	}

   function fGetSCOResponse()
   {   
      if (!top.frames.frmSubmitData.document.formMain || !top.frames.frmSubmitData.document.formMain.TAErrorNo){
         return;
      }

      var sErrorNo = top.frames.frmSubmitData.document.formMain.TAErrorNo.value;

      if(sErrorNo.replace(/ /gi, '') != ''){
         window.clearInterval(gTimerId);
         top.frames.frmSubmitData.location.replace(gAppURL + "/dummy.html");
                  
         if (bExited)
         {
         	fRefreshOpenerAndClose();
         }
      }
	}

	function fGetCount(lSCO, lUserNodePath){
		//cmi__objectives___count										n.id
		//cmi__interactions___count									n.id
		//cmi__interactions___n__objectives___count				n.id
		//cmi__interactions___n__correct_responses___count		n.pattern

		var sPreText = lUserNodePath.substr(0, lUserNodePath.indexOf("___count"));
		var sPostText
		if (lUserNodePath.indexOf("correct_responses") != -1){
			sPostText = "pattern";
		}
		else{
			sPostText = "id";
		}
		
		var i = -1;
		var sRetVal;
		for(;;){
			eval("sRetVal = lSCO." + sPreText + "___" + (++i) + "__" + sPostText);
			if (fIsUndefined(sRetVal)){
				break;
			}
		}
		
		return i;
	}

	function fFavoritesToXML()
	{
		var sRetVal="<favorites>";

		for (var i=0;i<arrFavorites.length;i++){
			sRetVal = sRetVal+"<favorite NodeId='"+arrFavorites[i].identifier+"'><![CDATA["+arrFavorites[i].Title+"]]></favorite>";
		}
		
		return sRetVal + "</favorites>";
	}
	
	function fArrSCOToXML()
	{
		var sRetVal="<Root>" + fFavoritesToXML();

		for (var i=0;i<arrSCO.length;i++){
			sRetVal = sRetVal + fSCOToXML(arrSCO[i].identifier);
		}

		return sRetVal + "</Root>";
	}

   function fSCOToXML(lIdentifier){
   	var sPreText;
   	var oSCO = fGetSCO(lIdentifier);

   	var iCnt = fGetCount(oSCO, "cmi__objectives___count");
		var sObjectives = "	<objectives>\n" +
						  		"		<_count>" + iCnt + "</_count>\n";
						  
  		sPreText = "cmi__objectives___";
   	for(var i=0;i<iCnt;i++){
   		sObjectives = 	sObjectives + "<_" + i + ">\n" +
   							"	<id>" + fGetValueFromSCO(oSCO, sPreText + i + "__id") + "</id>\n" + 
   							"	<score>\n" +
   							"		<raw>" + fGetValueFromSCO(oSCO, sPreText + i + "__score__raw") + "</raw>\n" + 
								"		<max>" + fGetValueFromSCO(oSCO, sPreText + i + "__score__max") + "</max>\n" + 
								"		<min>" + fGetValueFromSCO(oSCO, sPreText + i + "__score__min") + "</min>\n" + 
   							"	</score>\n" +
   							"	<status>" + fGetValueFromSCO(oSCO, sPreText + i + "__status") + "</status>\n" + 
   							"</_" + i + ">\n";
   	}
   	
   	sObjectives = 	sObjectives + "	</objectives>\n";
   	

		var sIntPreText = "";
		var iCnt;
		var j;
		var iIntCnt = fGetCount(oSCO, "cmi__interactions___count");

		var sInteractions = "	<interactions>\n" +
								  "		<_count>" + iIntCnt + "</_count>\n";

		sIntPreText = "cmi__interactions___";
		for(i=0;i<iIntCnt;i++){
			sInteractions = sInteractions + "<_" + i + ">\n" +
								"	<id>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__id") + "</id>\n";

			iCnt = fGetCount(oSCO, sIntPreText + i + "__objectives___count");
			sInteractions = sInteractions + "	<objectives>\n" +
								 "		<_count>" + iCnt + "</_count>\n";

				for(j=0;j<iCnt;j++){
					sInteractions = sInteractions + "	<_" + j + ">\n" +
									  "		<id>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__objectives___" + j + "__id") + "</id>\n" +
									  "	</_" + j + ">\n";

				}

			sInteractions = sInteractions + "	</objectives>\n" +
								 "	<time>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__time") + "</time>\n" +
								 "	<type>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__type") + "</type>\n";

			iCnt = fGetCount(oSCO, sIntPreText + i + "__correct_responses___count");
			sInteractions = sInteractions + "	<correct_responses>\n" +
								 "		<_count>" + iCnt + "</_count>\n";

				for(j=0;j<iCnt;j++){
					sInteractions = sInteractions + "	<_" + j + ">\n" +
									  "		<pattern>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__correct_responses___" + j + "__pattern") + "</pattern>\n" +
									  "	</_" + j + ">\n";

				}

			sInteractions = sInteractions + "	</correct_responses>\n" +
								 "	<weighting>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__weighting") + "</weighting>\n" +
								 "	<student_response>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__student_response") + "</student_response>\n" +
								 "	<result>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__result") + "</result>\n" +
								 "	<latency>" + fGetValueFromSCO(oSCO, sIntPreText + i + "__latency") + "</latency>\n" +
								 "</_" + i + ">\n";
		}

		sInteractions = 	sInteractions + "	</interactions>\n";


   	var sRetXML = 	"<sco identifier='" + lIdentifier + "'>\n<cmi>\n	<core>\n" +
							"		<student_id>" + fGetValueFromSCO(oSCO, "cmi__core__student_id") + "</student_id>\n" +
							"		<student_name>" + fGetValueFromSCO(oSCO, "cmi__core__student_name") + "</student_name>\n" +
							"		<lesson_location>" + fGetValueFromSCO(oSCO, "cmi__core__lesson_location") + "</lesson_location>\n" +
							"		<credit>" + fGetValueFromSCO(oSCO, "cmi__core__credit") + "</credit>\n" +
							"		<lesson_status>" + fGetValueFromSCO(oSCO, "cmi__core__lesson_status") + "</lesson_status>\n" +
							"		<entry>" + fGetValueFromSCO(oSCO, "cmi__core__entry") + "</entry>\n" +
							"		<score>\n" +
							"			<raw>" + fGetValueFromSCO(oSCO, "cmi__core__score__raw") + "</raw>\n" +
							"			<max>" + fGetValueFromSCO(oSCO, "cmi__core__score__max") + "</max>\n" +
							"			<min>" + fGetValueFromSCO(oSCO, "cmi__core__score__min") + "</min>\n" +
							"		</score>\n" +
							"		<total_time>" + fGetValueFromSCO(oSCO, "cmi__core__total_time") + "</total_time>\n" +
							"		<lesson_mode>" + fGetValueFromSCO(oSCO, "cmi__core__lesson_mode") + "</lesson_mode>\n" +
							"		<exit>" + fGetValueFromSCO(oSCO, "cmi__core__exit") + "</exit>\n" +
							"		<session_time>" + fGetValueFromSCO(oSCO, "cmi__core__session_time") + "</session_time>\n" +
							"	</core>\n" +
							"	<suspend_data>" + fGetValueFromSCO(oSCO, "cmi__suspend_data") + "</suspend_data>\n" +
							"	<launch_data><![CDATA[" + fGetValueFromSCO(oSCO, "cmi__launch_data") + "]]></launch_data>\n" +
							"	<comments>" + fGetValueFromSCO(oSCO, "cmi__comments") + "</comments>\n" +
							"	<comments_from_lms>" + fGetValueFromSCO(oSCO, "cmi__comments_from_lms") + "</comments_from_lms>\n" +
							sObjectives +
							"	<student_data>\n" +
							"		<mastery_score>" + fGetValueFromSCO(oSCO, "cmi__student_data__mastery_score") + "</mastery_score>\n" +
							"		<max_time_allowed>" + fGetValueFromSCO(oSCO, "cmi__student_data__max_time_allowed") + "</max_time_allowed>\n" +
							"		<time_limit_action>" + fGetValueFromSCO(oSCO, "cmi__student_data__time_limit_action") + "</time_limit_action>\n" +
							"	</student_data>\n" +
							"	<student_preference>\n" +
							"		<audio>" + fGetValueFromSCO(oSCO, "cmi__student_preference__audio") + "</audio>\n" +
							"		<language>" + fGetValueFromSCO(oSCO, "cmi__student_preference__language") + "</language>\n" +
							"		<speed>" + fGetValueFromSCO(oSCO, "cmi__student_preference__speed") + "</speed>\n" +
							"		<text>" + fGetValueFromSCO(oSCO, "cmi__student_preference__text") + "</text>\n" +
							"	</student_preference>\n" +
							sInteractions +
							"</cmi>\n</sco>\n";
	
		return sRetXML;
   }

//--------------------------------------------------------------------------------
//Get Set Functions
//--------------------------------------------------------------------------------

	function fGetValueFromMasterTemplate(lParam){
		eval("var sRetVal = gMasterTemplate." + lParam);
		if (fIsUndefined(sRetVal)){
			sRetVal = "";
		}
		
		return sRetVal;
	}

	function fGetValueFromSCO(lSCO, lParam){
		
		if (!lSCO){
			return "";
		}
		
		eval("var sRetVal = lSCO." + lParam);
		if (fIsUndefined(sRetVal)){
			sRetVal = "";
		}
		
		return sRetVal;
	}

	function fGetItemNodeObject(lIdentifier){
		var arrNodes = arrManifestNodes; //NTrees["COOLjsTree"].Nodes;
		for(var i=0;i<arrNodes.length;i++){
			if(arrNodes[i].identifier == lIdentifier){
				return arrNodes[i];
			}
		}
		return null;
	}

	function fGetValue(lNode, lParam){
		var sRetVal = lNode[lParam];
		if(sRetVal){
			return sRetVal;
		}
		else{
			return "";
		}
	}
	
	function fGetDataFromManifest(lParam){
		var sNodeId = fGetNode("Identifier");
		
		var oNode = fGetItemNodeObject(sNodeId);
		return fGetValue(oNode, lParam);
	}

	function fGetSCO(lIdentifier){
		for (var i=0;i<arrSCO.length;i++){
		
			if(arrSCO[i].identifier == lIdentifier){
				
				return arrSCO[i];
			}
		}
		
		return null;
	}
	
	function fGetCurrentSCO(){
		var sIdentifier = fGetNode("Identifier");
		return fGetSCO(sIdentifier);
	}
	
	function fNodeExistsInCurrentSco(lParam){
		var sRetVal;
		var oSCO = fGetCurrentSCO();

		if (oSCO){
			eval("sRetVal = oSCO." + lParam);
			return (!fIsUndefined(sRetVal));
		}
	}
	
	function fDirectGetDataFormUserDataXML(lParam){
		var sRetVal;
		var oSCO = fGetCurrentSCO();

		if (oSCO){
			eval("sRetVal = oSCO." + lParam);
			if (fIsUndefined(sRetVal)){
				sRetVal = "";
			}
		}

		return sRetVal;
	}

	function fDirectSetDataToUserDataXML(lParam, lData){
		var oSCO = fGetCurrentSCO();

		if (oSCO){
			eval("oSCO." + lParam + " = lData");
		}
	}

	function fGetNode(lParam){
		return gArrCurrentStatus[lParam];
	}

	function fSetNode(lParam, lVal){
		gArrCurrentStatus[lParam] = lVal;
		return true;
	}

//--------------------------------------------------------------------------
	function fOnExit(){
		if (gCourseLoaded){
			fSendRequest("Commit");
			bExited = true;
			fUpdateCourseTrackingDatabase();
		}
	}

	function fRefreshOpenerAndClose(){
		var OpenerObj = top.opener;
		var ParentObj = top.parent;
		if (OpenerObj){
			OpenerObj.location.reload();
		}
		ParentObj.close()
	}
	
	function fGetTotalCompletedNodes(){
		var iCompletedNodes = 0;
		var sStatus;

		for (var i=0;i<arrSCO.length;i++){
			sStatus = arrSCO[i].cmi__core__lesson_status;
			if(sStatus == "completed" || sStatus == "passed"){
				iCompletedNodes++;
			}
		}
		
		return iCompletedNodes;
	}
	
	function fUpdateCourseTrackingDatabase(){
		var iCompletedPages = fGetTotalCompletedNodes();
		var iTotalPages = gTotalManifestNodes;

		var sStatus = "Started";
		var sDate = "";
		
		if (iTotalPages == iCompletedPages){
			sStatus = "Completed";
			sDate = new Date();
		}

		var sTrans = "<Records>" +
					"<CourseId><![CDATA[" + gManifestId + "]]></CourseId>" +
					"<LearnerId><![CDATA[" + gStudentId + "]]></LearnerId>" +
					"<CourseName><![CDATA[" + gCourseName + "]]></CourseName>" +
					"<TotalNoOfPages><![CDATA[" + iTotalPages + "]]></TotalNoOfPages>" +
					"<NoOfPagesCompleted><![CDATA[" + iCompletedPages + "]]></NoOfPagesCompleted>" +
					"<Status><![CDATA[" + sStatus + "]]></Status>" +
					"</Records>";
		fSendDataToServer(sTrans, gAppURL + "/StandardActionsMain.jsp?Mode=FireAndReturn&PageId=LtLearnerCourseTracking");

		//call oXMLLIb.sendHTTP(oTransaction.XMLDocument, "../../../StandardActionsMain.asp?Mode=FireAndReturn&PageId=LtLearnerCourseTracking")
	}
	
	function fGoToBookmark(){	
		var sBookmark = gBookmark;

		if (sBookmark != ""){
			if(!confirm("Would you like to return to the last visited page ?")){
				sBookmark = "";
			}
		}

		if (sBookmark != ""){
			var oNode = fGetItemNodeObject(sBookmark);
			var lUrl =  fGetValue(oNode, "href");
			var lIdentifier =  fGetValue(oNode, "identifier");

             

			fOpenScoAsset(gContentPath+lUrl,lIdentifier)
		}
	}
	
	function fGoToLaunchSco(sSingleScoLaunch){	
		var sBookmark = sSingleScoLaunch;

       

		if (sBookmark != ""){
			var oNode = fGetItemNodeObject(sBookmark);
			var lUrl =  fGetValue(oNode, "href");
			var lIdentifier =  fGetValue(oNode, "identifier");
			
			fOpenScoAsset(gContentPath+lUrl,lIdentifier)
		}
	}
	
	function fGetNextPrevItemNodeObject(lIdentifier, lCase){
		var arrNodes = arrManifestNodes; //NTrees["COOLjsTree"].Nodes;
		var i;
		for(i=0;i<arrNodes.length;i++){
			if(arrNodes[i].identifier == lIdentifier){
				break;
			}
		}
		
		switch(lCase){
			case 'Next':
				i++;
				if (i == arrNodes.length){
					return null;
				}
					
				return arrNodes[i];
			case 'Previous':
				i--
				if (i < 0){
					return null;
				}
				
				return arrNodes[i];
		}
		
		return null;
	}