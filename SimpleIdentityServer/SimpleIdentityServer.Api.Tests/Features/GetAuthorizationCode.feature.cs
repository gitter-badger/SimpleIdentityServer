﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SimpleIdentityServer.Api.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("GetAuthorizationCode")]
    public partial class GetAuthorizationCodeFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GetAuthorizationCode.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "GetAuthorizationCode", "As a resource owner and user of the client\nI should be able to retrieve an author" +
                    "ization code", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Whether the user is authenticated or not we want to re-authenticate him")]
        public virtual void WhetherTheUserIsAuthenticatedOrNotWeWantToRe_AuthenticateHim()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Whether the user is authenticated or not we want to re-authenticate him", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("scopes openid,PlanningApi are defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("the scopes openid,PlanningApi are assigned to the client MyHolidays", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "scope",
                        "response_type",
                        "client_id",
                        "redirect_uri",
                        "prompt"});
            table1.AddRow(new string[] {
                        "openid PlanningApi",
                        "code",
                        "MyHolidays",
                        "http://localhost",
                        "login"});
#line 12
 testRunner.When("requesting an authorization code", ((string)(null)), table1, "When ");
#line 17
 testRunner.Then("HTTP status code is 301", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
 testRunner.And("redirect to /Authenticate controller", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user is authenticated and we want to display only the consent screen")]
        public virtual void AUserIsAuthenticatedAndWeWantToDisplayOnlyTheConsentScreen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user is authenticated and we want to display only the consent screen", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.And("scopes openid,PlanningApi are defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("the scopes openid,PlanningApi are assigned to the client MyHolidays", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "UserId",
                        "UserName"});
            table2.AddRow(new string[] {
                        "habarthierry@loki.be",
                        "thabart"});
#line 24
 testRunner.And("a resource owner is authenticated", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "scope",
                        "response_type",
                        "client_id",
                        "redirect_uri",
                        "prompt"});
            table3.AddRow(new string[] {
                        "openid PlanningApi",
                        "code",
                        "MyHolidays",
                        "http://localhost",
                        "consent"});
#line 28
 testRunner.When("requesting an authorization code", ((string)(null)), table3, "When ");
#line 33
 testRunner.Then("HTTP status code is 301", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.And("redirect to /Consent controller", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user is not authenticated and we want to display only the consent screen")]
        public virtual void AUserIsNotAuthenticatedAndWeWantToDisplayOnlyTheConsentScreen()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user is not authenticated and we want to display only the consent screen", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 37
 testRunner.Given("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.And("scopes openid,PlanningApi are defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.And("the scopes openid,PlanningApi are assigned to the client MyHolidays", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "scope",
                        "response_type",
                        "client_id",
                        "redirect_uri",
                        "prompt"});
            table4.AddRow(new string[] {
                        "openid PlanningApi",
                        "code",
                        "MyHolidays",
                        "http://localhost",
                        "consent"});
#line 41
 testRunner.When("requesting an authorization code", ((string)(null)), table4, "When ");
#line 46
 testRunner.Then("HTTP status code is 301", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.And("redirect to /Authenticate controller", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user is not authenticated but we want to directly retrieve the authorization co" +
            "de into the callback")]
        public virtual void AUserIsNotAuthenticatedButWeWantToDirectlyRetrieveTheAuthorizationCodeIntoTheCallback()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user is not authenticated but we want to directly retrieve the authorization co" +
                    "de into the callback", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
 testRunner.Given("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 53
 testRunner.And("scopes openid,PlanningApi are defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.And("the scopes openid,PlanningApi are assigned to the client MyHolidays", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "scope",
                        "response_type",
                        "client_id",
                        "redirect_uri",
                        "prompt",
                        "state"});
            table5.AddRow(new string[] {
                        "openid PlanningApi",
                        "code",
                        "MyHolidays",
                        "http://localhost",
                        "none",
                        "state1"});
#line 56
 testRunner.When("requesting an authorization code", ((string)(null)), table5, "When ");
#line 60
 testRunner.Then("HTTP status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "error",
                        "state"});
            table6.AddRow(new string[] {
                        "login_required",
                        "state1"});
#line 61
 testRunner.And("the error returned is", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion