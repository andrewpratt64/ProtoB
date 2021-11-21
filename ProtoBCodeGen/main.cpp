/// @author Andrew Pratt
/// @brief ProtoBCodeGen
/// @file

#include <string>
#include <iostream>
#include <fstream>
#include <filesystem>


/// @brief Namespace to encapsulate local program
namespace ProtoBCodeGen
{
	/// Platform-independent character type
	typedef std::filesystem::path::value_type char_t;
	/// Platform-independent string type
	typedef std::filesystem::path::string_type string_t;
	/// Platform-independent output stream type
	typedef std::basic_ostream<char_t> ostream_t;


	/// @brief Correct number of command-line arguments expected by the app
	constexpr int EXPECTED_ARGC{ 3 };
	/// @brief Correct command-line format for running this application
	constexpr auto CMD_FMT{ "ProtoBCodeGen.exe <path to cpp project folder> <path to c# project folder>" };


	/// @brief Assert that a filepath is a directory at runtime
	/// @param[in] checkPath Filepath to check
	/// @param[in] errStream Error stream to write to if checkPath isn't a directory, may be
	///		nullptrif if no error messages are needed
	/// @return True if checkPath is NOT a directory, false if it is
	/// @detail Use this function to test if a filepath that's expected to be a directory
	///		is valid or not. This function returns false if the filepath isn't a directory
	///		to make bailing/throwing easier, for example: \cif (isDirExpectationFailedBy(foo)) return EXIT_FAILURE;\c
	///		In addition, if errStream is NOT nullptr, an error message will be written to
	///		errStream if the filepath isn't a directory.
	bool isDirExpectationFailedBy(const std::filesystem::path& checkPath, ostream_t* errStream);


	/// @brief Tests if the filepaths for the c++ and c# projects are invalid
	/// @param[in] cppPath Filepath of the c++ project's root directory 
	/// @param[in] csPath Filepath of the c# project's root directory
	/// @param[in] errStream Error stream to write to if paths are invalid, may be
	///		nullptr if if no error messages are needed
	/// @return True if paths are NOT valid, false if they are valid
	bool areProjectPathsInvalid(
		const std::filesystem::path& cppPath,
		const std::filesystem::path& csPath,
		ostream_t* errStream = nullptr
	);
	

	/// @brief Call to generate code
	/// @param[in] cppPath Filepath of the c++ project's root directory 
	/// @param[in] csPath Filepath of the c# project's root directory
	/// /// @param[in] errStream Error stream to write to if an error occurs, may be
	///		nullptr if if no error messages are needed
	/// @return True if code generated successfully, false otherwise
	bool genCode(
		const std::filesystem::path& cppPath,
		const std::filesystem::path& csPath,
		ostream_t* errStream = nullptr
	);
}



/// @brief Program entry point
int main(int argc, char* argv[])
{
	// Ensure correct number of args were passed
	if (argc != ProtoBCodeGen::EXPECTED_ARGC)
	{
		// If incorrect args were passed, display an error message and bail
		std::cerr << "Invalid arguments" << std::endl
			<< argc << " arguments were passed, " << ProtoBCodeGen::EXPECTED_ARGC << " were expected." << std::endl
			<< "FORMAT: " << ProtoBCodeGen::CMD_FMT << std::endl;
		return EXIT_FAILURE;
	}
	



	return EXIT_SUCCESS;
}



bool ProtoBCodeGen::isDirExpectationFailedBy(const std::filesystem::path& checkPath, ProtoBCodeGen::ostream_t* errStream)
{
	// Return false if checkPath is a directory
	if (std::filesystem::is_directory(checkPath))
		return false;
	
	// If checkPath is NOT a directory, and and output error stream was given,
	//	write an error message
	if (errStream)
		*errStream << "Path is not a directory, but was expected to be one: \"" << checkPath.c_str() << '\"' << std::endl;

	// Return true if checkPath is NOT a directory
	return true;
}


bool ProtoBCodeGen::areProjectPathsInvalid(
	const std::filesystem::path& cppPath,
	const std::filesystem::path& csPath,
	ProtoBCodeGen::ostream_t* errStream
)
{
	return isDirExpectationFailedBy(cppPath, errStream) || isDirExpectationFailedBy(csPath, errStream);
}


bool ProtoBCodeGen::genCode(const std::filesystem::path& cppPath, const std::filesystem::path& csPath, ProtoBCodeGen::ostream_t* errStream)
{
	// Bail if any paths are invalid
	if (areProjectPathsInvalid(cppPath, csPath))
	return false;
}