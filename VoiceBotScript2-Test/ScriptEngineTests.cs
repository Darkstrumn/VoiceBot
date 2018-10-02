using NUnit.Framework;
using Shouldly;
using VoiceBotScriptTemplate.Includes.VoiceBotSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceBotScriptTemplate.Includes;
using System.Reflection;

namespace VoiceBotScriptTemplate.Includes.VoiceBotSupportClasses.Tests
    {
    [TestFixture(Description = "Script Engine Atomic Function Tests")]
    public class ScriptEngineTests
        {
        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");\nVoiceBotScriptTemplate.BFS.Speech.TextToSpeech(\"Commander - Script Engine Eval Unit Test Passes.\");", "", ExpectedResult = true, Description = "Eval Test")]
        public object Eval_Should_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            //ACT
            var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);

            //ASSERT
            result.ShouldNotBeNull();
            return result;
            }

        [Test(Description = "Bfs.Speech.TextToSpeech Access Test")]
        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");\nVoiceBotScriptTemplate.BFS.Speech.TextToSpeech(\"Commander - BFS Speech TTS Unit Test Passes.\");", "", ExpectedResult = true, Description = "Bfs.Speech.TextToSpeech Access Test")]
        public object Eval_Should_Have_Access_To_TTS_And_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            //ACT
            var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);

            //ASSERT
            result.ShouldNotBeNull();
            return result;
            }

        [Test(Description = "Bfs.ScriptSettings RW Access Test")]
        [TestCase("TriggerLogic", "BFS.ScriptSettings.WriteValue(\"UnitTest\", \"UnitTestPasses\");_bln_result = (BFS.ScriptSettings.ReadValue(\"UnitTest\") == \"UnitTestPasses\");\nVoiceBotScriptTemplate.BFS.Speech.TextToSpeech(\"Commander - BFS Script Settings Read Write Unit Test Passes.\");", "", ExpectedResult = true, Description = "Bfs.ScriptSettings RW Access Test")]
        public object Eval_Should_Have_RW_Access_To_Mock_Registry_And_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            //ACT
            var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);

            //ASSERT
            result.ShouldNotBeNull();
            return result;
            }

        [Test(Description = "Bfs.Input.SendKeys Access Test")]
        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");\nVoiceBotScriptTemplate.BFS.Input.SendKeys(\"Commander - SendKeys Unit Test Passes.\");\nVoiceBotScriptTemplate.BFS.Speech.TextToSpeech(\"Commander - BFS Input SendKeys Unit Test Passes.\");", "", ExpectedResult = true, Description = "Bfs.Input.SendKeys Access Test")]
        public object Eval_Should_Have_SendKeys_Access_And_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            //ACT
            var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);

            //ASSERT
            result.ShouldNotBeNull();
            return result;
            }

        [TestCase("TriggerLogic", "BFS.Speech.TextToSpeech(\"Commander - Voice Bot Script Template alias - Unit Test Passes.\");\n_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");", "", ExpectedResult = true, Description = "Bfs Alias Access Test")]
        public object Eval_Should_Have_Access_To_BfsAlias_And_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            //ACT
            var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);

            //ASSERT
            result.ShouldNotBeNull();
            return result;
            }

        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");", "", ExpectedResult = true, Description = "CsCodeAssembler Test")]
        public object CsCodeAssembler_Should_Compile_And_Return_Valid_Assembly_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            StringBuilder sb_script_core = new StringBuilder("");
            var class_name = codeBlockName; //<<--alias for clarity
            var function_name = "{CLASS}.Run".Replace("{CLASS}", class_name);
            var code = "";
            var code_template = @"
//=============================================================================
//=={CODEBLOCKNAME} - Unit Test
//=============================================================================
using System;
using System.Drawing;
namespace Includes
{
    public interface ITriggerLogic
    {
    bool Run(IntPtr windowHandle);
    }
    //-------------------------------------------------------------------------
    public class TriggerLogic : ITriggerLogic
    {
      private bool _bln_result = true;
      private string _error = ""ok"";
      private string _diagnostics = """";
      public TriggerLogic()
          {;}
      public bool Result{get{return(_bln_result);}}
      public string LastError{get{return(_error);}}
      public string DiagnosticsMessage{get{return(_diagnostics);}}
	    public bool Run(IntPtr windowHandle)
	    {
        try
            {
		    {CODEBLOCK}
            }
        catch(Exception error)
            {
            _error = ""TriggerLogic: fail - {ERROR}."".Replace(""{ERROR}"", error.Message);
            }

        return(Result);
	    }
    }
}";
            /*
            execute VHP's (variable hardpoints), allows us to add token handling to the code builder, can be a one-liner,
            but is clearer broken down. Note: order is important.
            */
            code = code_template.Replace("{CODEBLOCK}", codeBlock);
            code = code.Replace("{CODEBLOCKNAME}", codeBlockName);
            code = code.Replace("{REFERENCES}", Includes.VoiceBotSupportClasses.Constants.default_references);
            sb_script_core.Append(code);

            //ACT
            var assembly_library_code = ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);

            //ASSERT
            assembly_library_code.ShouldNotBeNull();
            return (assembly_library_code != null);
            }

        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");", "", ExpectedResult = true, Description = "ClassInstance Test")]
        public object CreateClassInstance_Should_Return_Class_Instance_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            StringBuilder sb_script_core = new StringBuilder("");
            var obj_parameters_array = new object[] { };
            var class_name = codeBlockName; //<<--alias for clarity
            var function_name = "{CLASS}.Run".Replace("{CLASS}", class_name);
            var code = "";

            var fullname = "Includes." + class_name;
            var bln_ignore_case = false;

            var code_template = @"
//=============================================================================
//=={CODEBLOCKNAME} - Unit Test
//=============================================================================
using System;
using System.Drawing;
namespace Includes
{
    public interface ITriggerLogic
    {
    bool Run(IntPtr windowHandle);
    }
    //-------------------------------------------------------------------------
    public class TriggerLogic : ITriggerLogic
    {
      private bool _bln_result = true;
      private string _error = ""ok"";
      private string _diagnostics = """";
      public TriggerLogic()
          {;}
      public bool Result{get{return(_bln_result);}}
      public string LastError{get{return(_error);}}
      public string DiagnosticsMessage{get{return(_diagnostics);}}
	    public bool Run(IntPtr windowHandle)
	    {
        try
            {
		    {CODEBLOCK}
            }
        catch(Exception error)
            {
            _error = ""TriggerLogic: fail - {ERROR}."".Replace(""{ERROR}"", error.Message);
            }

        return(Result);
	    }
    }
}";
            /*
            execute VHP's (variable hardpoints), allows us to add token handling to the code builder, can be a one-liner,
            but is clearer broken down. Note: order is important.
            */
            code = code_template.Replace("{CODEBLOCK}", codeBlock);
            code = code.Replace("{CODEBLOCKNAME}", codeBlockName);
            code = code.Replace("{REFERENCES}", Includes.VoiceBotSupportClasses.Constants.default_references);
            sb_script_core.Append(code);
            System.Reflection.BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance);
            var assembly_library_code = ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);
            dynamic obj_class = (dynamic)ScriptEngine.CreateClassInstance(assembly_library_code, class_name, obj_parameters_array);

            //ACT
            var classInstance = assembly_library_code.CreateInstance(fullname, bln_ignore_case, flags, null, obj_parameters_array, null, new object[] { });

            //ASSERT
            //<MSTest>Assert.IsNotNull(classInstance);
            classInstance.ShouldNotBeNull();
            return (classInstance != null);
            }

        [TestCase("TriggerLogic", "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");", "", ExpectedResult = true, Description = "Compiled Function Call Test")]
        public object CallFunction_Should_Compile_And_Invoke_Method_And_Return_True_Test(string codeBlockName, string codeBlock, string codeBlockReferences)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();

            StringBuilder sb_script_core = new StringBuilder("");
            var obj_parameters_array = new object[] { windowHandle };
            var class_name = codeBlockName; //<<--alias for clarity
            var function_name = "Run";
            var fullFunction_name = "{CLASS}.{FUNCTION}".Replace("{CLASS}", class_name).Replace("{FUNCTION}", function_name);
            var code = "";

            var fullname = "Includes." + class_name;
            //var bln_ignore_case = false;

            var code_template = @"
//=============================================================================
//=={CODEBLOCKNAME} - Unit Test
//=============================================================================
using System;
using System.Drawing;
namespace Includes
{
    public interface ITriggerLogic
    {
    bool Run(IntPtr windowHandle);
    }
    //-------------------------------------------------------------------------
    public class TriggerLogic : ITriggerLogic
    {
      private bool _bln_result = true;
      private string _error = ""ok"";
      private string _diagnostics = """";
      public TriggerLogic()
          {;}
      public bool Result{get{return(_bln_result);}}
      public string LastError{get{return(_error);}}
      public string DiagnosticsMessage{get{return(_diagnostics);}}
	    public bool Run(IntPtr windowHandle)
	    {
        try
            {
		    {CODEBLOCK}
            }
        catch(Exception error)
            {
            _error = ""TriggerLogic: fail - {ERROR}."".Replace(""{ERROR}"", error.Message);
            }

        return(Result);
	    }
    }
}";
            /*
            execute VHP's (variable hardpoints), allows us to add token handling to the code builder, can be a one-liner,
            but is clearer broken down. Note: order is important.
            */
            code = code_template.Replace("{CODEBLOCK}", codeBlock);
            code = code.Replace("{CODEBLOCKNAME}", codeBlockName);
            code = code.Replace("{REFERENCES}", Includes.VoiceBotSupportClasses.Constants.default_references);
            sb_script_core.Append(code);

            var assembly_library_code = ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);
            var classInstance = assembly_library_code.CreateInstance(fullname);
            var type_instance_type = classInstance.GetType();
            var method_info = type_instance_type.GetMethods().Where(m => m.Name == function_name).FirstOrDefault();


            //ACT
            object obj_return = method_info.Invoke(classInstance, obj_parameters_array);

            //ASSERT
            //<MSTest>Assert.IsTrue((bool)obj_return);//<<--expected object for this test is a bool true for the logic:'_bln_result = ("UnitTest" + "Passes" == "UnitTestPasses");'
            obj_return.ShouldNotBeNull();//<<--expected object for this test is a bool true for the logic:'_bln_result = ("UnitTest" + "Passes" == "UnitTestPasses");'
            return (obj_return);
            }

        //[Test()]
        //public void GetIncludeTest()
        //  {
        //  IntPtr windowHandle = new IntPtr();
        //  var include = "Include_DarkLibs";
        //  var references = Constants.default_references;
        //  var includeScript = BFS.ScriptSettings.ReadValue(include);
        //  var result = ((Assembly)ScriptEngine.GetInclude(windowHandle, include, references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];
        //  Assert.AreEqual(result.FullName, "Includes.Speech");
        //  }

        [TestCase("Unit Test", ExpectedResult = true, Description = "Base64Encode Encoder Test")]
        public object Base64Encode_Should_Encode_Proper_Test(string inputData)
            {
            //ARRANGE
            var testData = inputData;

            //ACT
            var encodedData = ScriptEngine.Base64Encode(testData);

            //ASSERT
            encodedData.ShouldNotBeEmpty();
            return (encodedData != testData);
            }

        [TestCase("Unit Test", ExpectedResult = "Unit Test", Description = "Base64Decode Decode Test")]
        public object Base64Decode_Should_Decode_Proper_Test(string inputData)
            {
            //ARRANGE
            var testData = inputData;
            var encodedData = ScriptEngine.Base64Encode(testData);

            //ACT
            var decodedData = ScriptEngine.Base64Decode(encodedData);

            //ASSERT
            decodedData.ShouldNotBeEmpty();
            decodedData.ShouldNotBe(encodedData);
            return decodedData;
            }

        [TestCase("Include_DarkLibs", ExpectedResult = "Includes.Speech", Description = "LoadInclude Load Test")]
        public object LoadInclude_Should_Load_Include_And_FullName_Should_Match_Include_Test(string include)
            {
            //ARRANGE
            IntPtr windowHandle = new IntPtr();
            var references = Constants.default_references;

            //ACT
            var result = ((Assembly)ScriptEngine.LoadInclude(windowHandle, include, references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];

            //ASSERT
            result.FullName.ShouldNotBeNull();
            return result.FullName;
            }
        }
    }