using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Security;
using System.Security.Policy;

namespace CustomSetupActions
{
    [RunInstaller(true)]
    public partial class CASPolicyInstaller : Installer
    {
        public CASPolicyInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            PolicyLevel ent;
            PolicyLevel mach;
            PolicyLevel user;
            string sAssemblyPath = this.Context.Parameters["custassembly"];
            //string sAssemblyPath = this.Context.Parameters["XWord.dll"];
            System.Collections.IEnumerator policies = SecurityManager.PolicyHierarchy();
            policies.MoveNext();
            ent = (PolicyLevel)policies.Current;
            policies.MoveNext();
            mach = (PolicyLevel)policies.Current;
            policies.MoveNext();
            user = (PolicyLevel)policies.Current;

            PermissionSet fullTrust = user.GetNamedPermissionSet("FullTrust");
            PolicyStatement statement = new PolicyStatement(fullTrust, PolicyStatementAttribute.Nothing);
            UrlMembershipCondition condition = new UrlMembershipCondition(sAssemblyPath);
            CodeGroup group = new UnionCodeGroup(condition, statement);
            group.Name = "TestWordAddInCS";
            user.RootCodeGroup.AddChild(group);
            SecurityManager.SavePolicy();
            
            base.Install(stateSaver);
        }
    }

   
}