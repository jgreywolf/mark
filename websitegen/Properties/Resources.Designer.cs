﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace websitegen.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("websitegen.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copyright &amp;copy; intfrog.github.io.
        /// </summary>
        internal static string footer {
            get {
                return ResourceManager.GetString("footer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;ul&gt;
        ///    &lt;li&gt;&lt;a href=&quot;#&quot;&gt;Home&lt;/a&gt;&lt;/li&gt;
        ///    &lt;li&gt;&lt;a href=&quot;#&quot;&gt;News&lt;/a&gt;&lt;/li&gt;
        ///    &lt;li&gt;&lt;a href=&quot;#&quot;&gt;Contact&lt;/a&gt;&lt;/li&gt;
        ///    &lt;li&gt;&lt;a href=&quot;#&quot;&gt;About&lt;/a&gt;&lt;/li&gt;
        ///&lt;/ul&gt;.
        /// </summary>
        internal static string header {
            get {
                return ResourceManager.GetString("header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     header ul {
        ///        list-style-type: none;
        ///        margin: 0;
        ///        padding: 0;
        ///        overflow: hidden;
        ///        background-color: grey;
        ///        position: fixed;
        ///        top: 0;
        ///        width: 100%;
        ///    }
        ///    header li {
        ///        float: left;
        ///    }
        ///    header li a {
        ///        display: block;
        ///        color: white;
        ///        text-align: center;
        ///        padding: 6px 16px;
        ///        text-decoration: none;
        ///    }
        ///    header li a:hover:not(.active) {
        ///        background-color: #333;
        ///    }
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string style {
            get {
                return ResourceManager.GetString("style", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to baseUrl=https://raw.githubusercontent.com/intfrog/intfrog.github.io/master
        ///title=我的笔记.
        /// </summary>
        internal static string websitegen {
            get {
                return ResourceManager.GetString("websitegen", resourceCulture);
            }
        }
    }
}
