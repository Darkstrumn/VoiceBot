using NUnit.Framework;
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
  [TestFixture()]
  public class ScriptEngineTests
    {
    [Test()]
    public void EvalTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");";
      var codeBlockReferences = "";

      var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);
      Assert.AreEqual(result, true);
      }

    [Test()]
    public void BfsTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");\nVoiceBotScriptTemplate.BFS.Speech.TextToSpeech(\"Commander - Unit Test Passes.\");";
      var codeBlockReferences = "";

      var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);
      Assert.AreEqual(result, true);
      }

    [Test()]
    public void BfsAliasTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "BFS.Speech.TextToSpeech(\"Commander - Voice Bot Script Template alias - Unit Test Passes.\");\n_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");";
      var codeBlockReferences = "";

      var result = ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);
      Assert.AreEqual(result, true);
      }

    [Test()]
    public void CsCodeAssemblerTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");";
      var codeBlockReferences = "";

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
      var assembly_library_code = ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);
      Assert.IsNotNull(assembly_library_code);
      }

    [Test()]
    public void CreateClassInstanceTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");";
      var codeBlockReferences = "";

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
      var classInstance = assembly_library_code.CreateInstance(fullname, bln_ignore_case, flags, null, obj_parameters_array, null, new object[] { });
      Assert.IsNotNull(classInstance);
      }

    [Test()]
    public void CallFunctionTest()
      {
      IntPtr windowHandle = new IntPtr();
      var codeBlockName = "TriggerLogic";
      var codeBlock = "_bln_result = (\"UnitTest\" + \"Passes\" == \"UnitTestPasses\");";
      var codeBlockReferences = "";

      StringBuilder sb_script_core = new StringBuilder("");
      var obj_parameters_array = new object[] { windowHandle };
      var class_name = codeBlockName; //<<--alias for clarity
      var function_name = "Run";
      var fullFunction_name = "{CLASS}.{FUNCTION}".Replace("{CLASS}", class_name).Replace("{FUNCTION}", function_name);
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

      var assembly_library_code = ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);
      var classInstance = assembly_library_code.CreateInstance(fullname);
      var type_instance_type = classInstance.GetType();
      var method_info = type_instance_type.GetMethods().Where(m => m.Name == function_name).FirstOrDefault();

      object obj_return = method_info.Invoke(classInstance, obj_parameters_array);
      Assert.IsTrue((bool)obj_return);//<<--expected object for this test is a bool true for the logic:'_bln_result = ("UnitTest" + "Passes" == "UnitTestPasses");'
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

    [Test()]
    public void Base64DecodeTest()
      {
      var testData = "Unit Test";
      var encodedData = ScriptEngine.Base64Encode(testData);
      var decodedData = ScriptEngine.Base64Decode(encodedData);

      Assert.AreEqual(testData, decodedData);
      }

    [Test()]
    public void Base64EncodeTest()
      {
      var testData = "Unit Test";
      var encodedData = ScriptEngine.Base64Encode(testData);

      Assert.IsNotEmpty(encodedData);
      Assert.AreNotEqual(testData, encodedData);
      }

    [Test()]
    public void LoadIncludeTest()
      {
      IntPtr windowHandle = new IntPtr();
      var include = "Include_DarkLibs";
      var references = Constants.default_references;
      var result = ((Assembly)ScriptEngine.LoadInclude(windowHandle, include, references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];
      Assert.AreEqual(result.FullName, "Includes.Speech");
      }
    }
  }