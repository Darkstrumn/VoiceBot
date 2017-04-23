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

      var result = Includes.VoiceBotSupportClasses.ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);
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
      object[] obj_parameters_array;
      var class_name = "TriggerLogic";
      var function_name = "{CLASS}.Run".Replace("{CLASS}", class_name);
      var code = "";
      var code_template = @"
//=============================================================================
//=={CODEBLOCKNAME}
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
      var assembly_trigger_logic = (Assembly)ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), codeBlockReferences);
      Assert.IsNotNull(assembly_trigger_logic);
      }

    [Test()]
    public void CreateClassInstanceTest()
      {
      Assert.Fail();
      }

    [Test()]
    public void CallFunctionTest()
      {
      Assert.Fail();
      }

    [Test()]
    public void GetIncludeTest()
      {
      Assert.Fail();
      }

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
      Assert.Fail();
      }
    }
  }