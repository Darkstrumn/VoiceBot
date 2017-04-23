using NUnit.Framework;
using VoiceBotScriptTemplate.Includes.VoiceBotSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceBotScriptTemplate.Includes;

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
      Assert.Fail();
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