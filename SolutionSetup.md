####Andrew Pratt 2021
###SOLUTION SETUP FOR ProtoB

- All cpp projects
	- Project Properties
		- General
			- Platform Toolset = Visual Studio 2019 (v142)
			- C++ Language Standard = *C++20*
			- C Language Standard = ISO C17 (2018) Standard (/std:c17)
			- Output Directory = $(ProjectDir)bin\$(Configuration)\net5.0\
		- C/C++
			- Precompiled Headers
				- Precompiled Header = Not Using Precompiled Headers
				- Precompiled Header File = *leave empty*
- All c# projects
	- Project Properties
		- Application Properties
			- Target framework = .NET 5.0
		- Build
			- General
				- Platform target = x64
				- Nullable: enable
				- Allow unsafe code = true
			- Output
				- Output path = <MAY HAVE TO BE SET MANUALLY(?)>
			- Debug
				- Enable native code debugging = true
- ProtoB
	- Project References
		- ProtoBSrcGen
			- Reference Output Assembly = No
	- AssemblyInfo1.cs
		- [assembly: InternalsVisibleTo("ProtoBUnitTest")]
- ProtoBCodeGen (CURRENTLY EXCLUDED FROM BUILD)
	- Project Properties
		- Debugging
			- Command Arguments = $(SolutionDir)ProtoBCpp $(SolutionDir)ProtoB
- ProtoBCpp
	- Project Properties
		- Build Events
			- Post-Build Event
				- Command Line = xcopy "$(OutputPath)*" "$(SolutionDir)ProtoB\bin\$(Configuration)\net5.0\" /y
				- Use In Build = Yes
- ProtoBCppUnitTesting
	- Project Properties
		- VC++ Directories
			- Include Directories
				- $(SolutionDir)ProtoBCpp
- ProtoBSrcGen (CURRENTLY EXCLUDED FROM BUILD)
	- Nuget Packages
		- Microsoft.CodeAnalysis.CSharp
			- Version = Latest stable (4.0.1 as of 11/9/2021)
		- Microsoft.CodeAnalysis.Analyzers
			- Version = Latest stable (3.3.3 as of 11/9/2021)
- ProtoBUnitTesting