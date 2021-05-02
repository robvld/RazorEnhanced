﻿using Microsoft.CSharp;
using Microsoft.Scripting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RazorEnhanced
{
    class CSharpEngine
    {
        private static CompilerParameters m_parameters = null;
        private static CSharpEngine m_instance = null;

        public static CSharpEngine Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CSharpEngine();
                }
                return m_instance;
            }
        }

        private CSharpEngine()
        {
            m_parameters = new CompilerParameters();
            List<string> assemblies = GetReferenceAssemblies();
            foreach (string assembly in assemblies)
            {
                m_parameters.ReferencedAssemblies.Add(assembly);
            }

            m_parameters.GenerateInMemory = true; // True - memory generation, false - external file generation
            m_parameters.GenerateExecutable = false; // True - exe file generation, false - dll file generation
            m_parameters.TreatWarningsAsErrors = false; // Set whether to treat all warnings as errors.
            m_parameters.WarningLevel = 4; // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings
            //parameters.CompilerOptions = "/optimize"; // Set compiler argument to optimize output.
        }

        private List<string> GetReferenceAssemblies()
        {
            List<string> list = new List<string>();

            string path = Path.Combine(Assistant.Engine.RootPath, "Scripts", "Assemblies.cfg");

            if (File.Exists(path))
            {
                using (StreamReader ip = new StreamReader(path))
                {
                    string line;

                    while ((line = ip.ReadLine()) != null)
                    {
                        if (line.Length > 0 && !line.StartsWith("#"))
                            list.Add(line);
                    }
                }
            }

            list.Add(Assistant.Engine.RootPath + "\\" + "RazorEnhanced.exe");
            return list;
        }

        private bool ManageCompileResult(CompilerResults results, out StringBuilder errorwarnings)
        {
            bool has_error = true;

            StringBuilder sb = new StringBuilder();
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) at line {1}: {2}", error.ErrorNumber, error.Line, error.ErrorText));
                }
            }
            else
            {
                has_error = false;

                if (results.Errors.HasWarnings)
                {
                    foreach (CompilerError warning in results.Errors)
                    {
                        sb.AppendLine(String.Format("Warning ({0}) at line {1}: {2}", warning.ErrorNumber, warning.Line, warning.ErrorText));
                    }
                }
            }
            errorwarnings = sb;
            return has_error;
        }

        public bool CompileFromText(string source, out StringBuilder errorwarnings, out Assembly assembly)
        {
            // When compiler is invoked from the editor it's always in debug mode
            m_parameters.IncludeDebugInformation = true; // Build in debug
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource(m_parameters, source); // Compiling

            assembly = null;
            bool has_error = ManageCompileResult(results, out errorwarnings);
            if (has_error)
            {
                var error = results.Errors[0];
                var a = new SourceLocation(0, error.Line, error.Column);
                throw new SyntaxErrorException(error.ErrorText, results.PathToAssembly, error.ErrorNumber, "", new SourceSpan(a, a), 0, Severity.Error);
            }
            else
            {
                assembly = results.CompiledAssembly;
            }
            return has_error;
        }

        public bool CompileFromFile(string path, bool debug, out StringBuilder errorwarnings, out Assembly assembly)
        {
            m_parameters.IncludeDebugInformation = debug; // Build in DEBUG or RELEASE

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromFile(m_parameters, path); // Compiling

            assembly = null;
            bool has_error = ManageCompileResult(results, out errorwarnings);
            if (!has_error)
            {
                assembly = results.CompiledAssembly;
            }
            return has_error;
        }

        public void Execute(Assembly assembly)
        {
            Type program = assembly.GetType("RazorEnhanced.Script");

            // This is important for methods visibility. Check if all of these flags are really needed.
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

            MethodInfo run = program.GetMethod("Run", bf);
            object scriptInstance = Activator.CreateInstance(run.DeclaringType);
            run.Invoke(scriptInstance, null);
        }

    }
}
