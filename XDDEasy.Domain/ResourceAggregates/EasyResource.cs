using System;
using System.Globalization;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Helper;

namespace XDDEasy.Domain.ResourceAggregates
{
    public static class EasyResource
    {
        private static readonly IResourceProvider Provider;

        static EasyResource()
        {
            var container =
            ((AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver).Container;
            Provider = container.Resolve<IResourceProvider>();
        }

        public static string Value(string key)
        {
            return Provider.GetResource(key, LangHelper.GetLanguage()) as string;
        }



        public static String Exception_InvalidAccount
        {
            get
            {
                return Provider.GetResource("Exception_InvalidAccount", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_CannotChangePassword
        {
            get
            {
                return Provider.GetResource("Exception_CannotChangePassword", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_AccountRemoved_CannotLogin
        {
            get
            {
                return Provider.GetResource("Exception_AccountRemoved_CannotLogin", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_WhenCreateingAccount_EmptySchoolId
        {
            get
            {
                return Provider.GetResource("Exception_WhenCreateingAccount_EmptySchoolId", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_StudentIdAlreadyUsed
        {
            get
            {
                return Provider.GetResource("Exception_StudentIdAlreadyUsed", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_ReadOnly_Yourself_Content
        {
            get
            {
                return Provider.GetResource("Exception_ReadOnly_Yourself_Content", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_DeleteGrade_CommonCoreInUsed
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_DeleteGrade_CommonCoreInUsed", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_GroupNotFound
        {
            get
            {
                return Provider.GetResource("Exception_GroupNotFound", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_PublicClinetId
        {
            get
            {
                return Provider.GetResource("Exception_PublicClinetId", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_ClassNotFound
        {
            get
            {
                return Provider.GetResource("Exception_ClassNotFound", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_SharedQuestionId_NotExist
        {
            get
            {
                return Provider.GetResource("Exception_SharedQuestionId_NotExist", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_TeacherNotFound
        {
            get
            {
                return Provider.GetResource("Exception_TeacherNotFound", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_AccountCant_delete
        {
            get
            {
                return Provider.GetResource("Exception_AccountCan't delete", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_AddSession_Without_Facebook
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_AddSession_Without_Facebook", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_SessionCannotEdit
        {
            get
            {
                return Provider.GetResource("Exception_SessionCannotEdit", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_RightCompletePercentage
        {
            get
            {
                return Provider.GetResource("Exception_RightCompletePercentage", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_StudentNotFound
        {
            get
            {
                return Provider.GetResource("Exception_StudentNotFound", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_DeleteSubject_CommonCore_AreadyUsed
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_DeleteSubject_CommonCore_AreadyUsed", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_TeamNameAlreadyUsed
        {
            get
            {
                return Provider.GetResource("Exception_TeamNameAlreadyUsed", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_CreateStudent
        {
            get
            {
                return Provider.GetResource("Exception_CreateStudent", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Student_Disconnected_Unsuccessfully
        {
            get
            {
                return Provider.GetResource("Exception_Student_Disconnected_Unsuccessfully", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Config
        {
            get
            {
                return Provider.GetResource("Config", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String ModelType
        {
            get
            {
                return Provider.GetResource("ModelType", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Api
        {
            get
            {
                return Provider.GetResource("Api", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Formatter
        {
            get
            {
                return Provider.GetResource("Formatter", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String MediaType
        {
            get
            {
                return Provider.GetResource("MediaType", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String ControllerName
        {
            get
            {
                return Provider.GetResource("ControllerName", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String ActionName
        {
            get
            {
                return Provider.GetResource("ActionName", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String ParameterNames
        {
            get
            {
                return Provider.GetResource("ParameterNames", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String ErrorMessage
        {
            get
            {
                return Provider.GetResource("ErrorMessage", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String DocumentPath
        {
            get
            {
                return Provider.GetResource("DocumentPath", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_InternalServerError
        {
            get
            {
                return Provider.GetResource("Exception_InternalServerError", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_RetrieveData
        {
            get
            {
                return Provider.GetResource("Exception_RetrieveData", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_EnumName_NotFound
        {
            get
            {
                return Provider.GetResource("Exception_EnumName_NotFound", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_AccountmappingAssemblies
        {
            get
            {
                return Provider.GetResource("Exception_AccountmappingAssemblies", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Accountapplication
        {
            get
            {
                return Provider.GetResource("Exception_Accountapplication", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_AccountKey
        {
            get
            {
                return Provider.GetResource("Exception_AccountKey", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Accountconnection
        {
            get
            {
                return Provider.GetResource("Exception_Accountconnection ", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Text
        {
            get
            {
                return Provider.GetResource("Text", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Accountentity
        {
            get
            {
                return Provider.GetResource("Exception_Accountentity ", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_BeginANewTransaction_RunningTransaction
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_BeginANewTransaction_RunningTransaction", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_BeginANewTransaction_CommitExisting
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_BeginANewTransaction_CommitExisting", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Incorrect_UserName_Password
        {
            get
            {
                return Provider.GetResource("Exception_Incorrect_UserName_Password", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_WrongPassword
        {
            get
            {
                return Provider.GetResource("Exception_WrongPassword", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_QuestionId_NotExist
        {
            get
            {
                return Provider.GetResource("Exception_QuestionId_NotExist", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_Delete_User
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_Delete_User", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_Delete
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_Delete", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String Exception_Cannot_Rollaback_Transaction
        {
            get
            {
                return Provider.GetResource("Exception_Cannot_Rollaback_Transaction", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Content
        {
            get
            {
                return Provider.GetResource("UI_Content", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ShowHelp
        {
            get
            {
                return Provider.GetResource("UI_ShowHelp", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_DefaultContentViewTip
        {
            get
            {
                return Provider.GetResource("UI_DefaultContentViewTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EditTip
        {
            get
            {
                return Provider.GetResource("UI_EditTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_DeleteTip
        {
            get
            {
                return Provider.GetResource("UI_DeleteTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ShareTip
        {
            get
            {
                return Provider.GetResource("UI_ShareTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_QuickViewTip
        {
            get
            {
                return Provider.GetResource("UI_QuickViewTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_CopyTip
        {
            get
            {
                return Provider.GetResource("UI_CopyTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_SearchTip
        {
            get
            {
                return Provider.GetResource("UI_SearchTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AddTip
        {
            get
            {
                return Provider.GetResource("UI_AddTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_CreateDeckTip
        {
            get
            {
                return Provider.GetResource("UI_CreateDeckTip", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_GradeLevel
        {
            get
            {
                return Provider.GetResource("UI_GradeLevel", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Subject
        {
            get
            {
                return Provider.GetResource("UI_Subject", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI__Type
        {
            get
            {
                return Provider.GetResource("UI_ Type", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Type
        {
            get
            {
                return Provider.GetResource("UI_Type", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Topic
        {
            get
            {
                return Provider.GetResource("UI_Topic", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ADD
        {
            get
            {
                return Provider.GetResource("UI_ADD", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Share
        {
            get
            {
                return Provider.GetResource("UI_Share", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_CreateDeck
        {
            get
            {
                return Provider.GetResource("UI_CreateDeck", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ClearSelected
        {
            get
            {
                return Provider.GetResource("UI_ClearSelected", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ALL
        {
            get
            {
                return Provider.GetResource("UI_ALL", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_System
        {
            get
            {
                return Provider.GetResource("UI_System", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_School
        {
            get
            {
                return Provider.GetResource("UI_School", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Shared
        {
            get
            {
                return Provider.GetResource("UI_Shared", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_PersonalOnly
        {
            get
            {
                return Provider.GetResource("UI_PersonalOnly", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Show10Entries
        {
            get
            {
                return Provider.GetResource("UI_Show10Entries", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Search
        {
            get
            {
                return Provider.GetResource("UI_Search", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Title
        {
            get
            {
                return Provider.GetResource("UI_Title", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_QuestionType
        {
            get
            {
                return Provider.GetResource("UI_QuestionType", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_DifficultyLevel
        {
            get
            {
                return Provider.GetResource("UI_DifficultyLevel", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_UpdatedTime
        {
            get
            {
                return Provider.GetResource("UI_UpdatedTime", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Scope
        {
            get
            {
                return Provider.GetResource("UI_Scope", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Actions
        {
            get
            {
                return Provider.GetResource("UI_Actions", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Edit
        {
            get
            {
                return Provider.GetResource("UI_Edit", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Delete
        {
            get
            {
                return Provider.GetResource("UI_Delete", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ShareQuestion
        {
            get
            {
                return Provider.GetResource("UI_ShareQuestion", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Preview
        {
            get
            {
                return Provider.GetResource("UI_Preview", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Copy
        {
            get
            {
                return Provider.GetResource("UI_Copy", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Showing_1_to_10__of__91_entries
        {
            get
            {
                return Provider.GetResource("UI_Showing_1_to_10 _of_ 91 entries", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_First
        {
            get
            {
                return Provider.GetResource("UI_First", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Previous
        {
            get
            {
                return Provider.GetResource("UI_Previous", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Next
        {
            get
            {
                return Provider.GetResource("UI_Next", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Last
        {
            get
            {
                return Provider.GetResource("UI_Last", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_TwoChoiceDescription
        {
            get
            {
                return Provider.GetResource("UI_TwoChoiceDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_MultipleChoiceDescription
        {
            get
            {
                return Provider.GetResource("UI_MultipleChoiceDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Fill_in_Blank_Description
        {
            get
            {
                return Provider.GetResource("UI_Fill_in_Blank_Description", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Write_DrawDescription
        {
            get
            {
                return Provider.GetResource("UI_Write_DrawDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Write_DrawDescription2
        {
            get
            {
                return Provider.GetResource("UI_Write_DrawDescription2", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EbookDescription
        {
            get
            {
                return Provider.GetResource("UI_EbookDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_VideoDescription
        {
            get
            {
                return Provider.GetResource("UI_VideoDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_WebResourceDescription
        {
            get
            {
                return Provider.GetResource("UI_WebResourceDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_DocumentDescription
        {
            get
            {
                return Provider.GetResource("UI_DocumentDescription", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_SingleChoice
        {
            get
            {
                return Provider.GetResource("UI_SingleChoice", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Property
        {
            get
            {
                return Provider.GetResource("UI_Property", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_CommonCore
        {
            get
            {
                return Provider.GetResource("UI_CommonCore", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Design
        {
            get
            {
                return Provider.GetResource("UI_Design", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Back
        {
            get
            {
                return Provider.GetResource("UI_Back", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_DoubleClickToChangeText
        {
            get
            {
                return Provider.GetResource("UI_DoubleClickToChangeText", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_UPLOAD_IMAGE
        {
            get
            {
                return Provider.GetResource("UI_UPLOAD_IMAGE", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EnterImageLink
        {
            get
            {
                return Provider.GetResource("UI_EnterImageLink", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AccessMyImage
        {
            get
            {
                return Provider.GetResource("UI_AccessMyImage", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Audio
        {
            get
            {
                return Provider.GetResource("UI_Audio", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EnterAudioLink
        {
            get
            {
                return Provider.GetResource("UI_EnterAudioLink", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AccessMyAudio
        {
            get
            {
                return Provider.GetResource("UI_AccessMyAudio", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AlternativeText
        {
            get
            {
                return Provider.GetResource("UI_AlternativeText", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Width
        {
            get
            {
                return Provider.GetResource("UI_Width", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Height
        {
            get
            {
                return Provider.GetResource("UI_Height", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Border
        {
            get
            {
                return Provider.GetResource("UI_Border", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EditSingleChoice
        {
            get
            {
                return Provider.GetResource("UI_EditSingleChoice", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Answer
        {
            get
            {
                return Provider.GetResource("UI_Answer", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Option
        {
            get
            {
                return Provider.GetResource("UI_Option", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AdvancedEdit
        {
            get
            {
                return Provider.GetResource("UI_AdvancedEdit", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ADDOption
        {
            get
            {
                return Provider.GetResource("UI_ADDOption", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_VerifyLayout
        {
            get
            {
                return Provider.GetResource("UI_VerifyLayout", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Save
        {
            get
            {
                return Provider.GetResource("UI_Save", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Cancel
        {
            get
            {
                return Provider.GetResource("UI_Cancel", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_MultipleChoice
        {
            get
            {
                return Provider.GetResource("UI_MultipleChoice", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_FillBlank
        {
            get
            {
                return Provider.GetResource("UI_FillBlank", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_AnswerSpace
        {
            get
            {
                return Provider.GetResource("UI_AnswerSpace", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_FillBlankProperties
        {
            get
            {
                return Provider.GetResource("UI_FillBlankProperties", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_LineHeight
        {
            get
            {
                return Provider.GetResource("UI_LineHeight", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_CorrectAnswer
        {
            get
            {
                return Provider.GetResource("UI_CorrectAnswer", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_WriteDraw
        {
            get
            {
                return Provider.GetResource("UI_WriteDraw", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_WriteDraw2
        {
            get
            {
                return Provider.GetResource("UI_WriteDraw2", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_STUDENT_DRAW_PANEL
        {
            get
            {
                return Provider.GetResource("UI_STUDENT_DRAW_PANEL", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_EBook
        {
            get
            {
                return Provider.GetResource("UI_EBook", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_InsertBefore
        {
            get
            {
                return Provider.GetResource("UI_InsertBefore", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Insert_After
        {
            get
            {
                return Provider.GetResource("UI_Insert After", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Sort_Page
        {
            get
            {
                return Provider.GetResource("UI_Sort Page", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Video
        {
            get
            {
                return Provider.GetResource("UI_Video", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_VideoLink
        {
            get
            {
                return Provider.GetResource("UI_VideoLink", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ChooseVideo
        {
            get
            {
                return Provider.GetResource("UI_ChooseVideo", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ChildContent
        {
            get
            {
                return Provider.GetResource("UI_ChildContent", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_ADDChildContent
        {
            get
            {
                return Provider.GetResource("UI_ADDChildContent", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_WebResource
        {
            get
            {
                return Provider.GetResource("UI_WebResource", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_Documents
        {
            get
            {
                return Provider.GetResource("UI_Documents", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_BrowseorUpload
        {
            get
            {
                return Provider.GetResource("UI_BrowseorUpload", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static String UI_SelectDocument
        {
            get
            {
                return Provider.GetResource("UI_SelectDocument", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

    }
}
