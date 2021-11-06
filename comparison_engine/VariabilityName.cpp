#include "VariabilityName.h"
#include "TranslitAlfavitTwo.h"
#include "ToUpper.h"
#include <iostream>
#include <fstream>
ToUpper toUpper;
VariabilityName::VariabilityName(std::string input) {
	VariabilityNameFile();
	VariabilityNameEng(input);
	
}

void VariabilityName::VariabilityNameFile() {
	std::ifstream in("variabilityName.txt");
	std::string s;
	std::string name;
	std::string varName;
	if (in.is_open())
	{
		while (!in.eof())
		{
			name.clear();
			varName.clear();
			std::getline(in, s);
			for (size_t i = 0; i < s.size(); i++)
				if (s[i] != '>')
					name += s[i];
				else {
					s.erase(0, i + 1);
					break;
				}
			varName = s;

			varNames.insert(std::pair<std::string, std::string>(name, varName));
		}
	}
	else {
		std::cout << "VariabilityName can't open! VariabilityNames not work!" << std::endl;
	}
	in.close();
}

void VariabilityName::VariabilityNameEng(std::string input) {
	std::string fnl[3];
	size_t indexFnl = 0;
	size_t index = 0;
	std::string tmp;
	int isFirstChar = 0;
	bool isSpace = false;
	for (char a : input) {
		if (a == ' ') {
			fnl[indexFnl++] = tmp;
			isFirstChar = 1;
			tmp.clear();
			isSpace = !isSpace;
			index++;
			continue;
		}
		try {
			if (((index % 2 == 1) && (!isSpace)) || (index % 2 == 0) && (isSpace))
				tmp += toUpper.toUpperAlfavit[a];
		}
		catch (const std::exception& e) {
			tmp += a;
		}
		index++;
		if (index >= input.size()) {
			fnl[indexFnl] = tmp;
		}
	}

	std::string firstName_t = Translit(fnl[0]);
	std::string name_t = Translit(fnl[1]);
	std::string lastName_t = Translit(fnl[2]);

	_FIO = fnl[0] + " " + fnl[1] + " " + fnl[2];
	_IFO = fnl[1] + " " + fnl[0] + " " + fnl[2];
	_IOF = fnl[1] + " " + fnl[2] + " " + fnl[0];
	_FI = fnl[0] + " " + fnl[1];
	_IF = fnl[1] + " " + fnl[0];

	_FIO_t = firstName_t + " " + name_t + " " + lastName_t;
	_IFO_t = name_t + " " + firstName_t + " " + lastName_t;
	_IOF_t = name_t + " " + lastName_t + " " + firstName_t;
	_FI_t = firstName_t + " " + name_t;
	_IF_t = name_t + " " + firstName_t;
}

std::string VariabilityName::Translit(std::string input) {
	std::string output;
	for (int i = 0; i < input.size(); ++i)
	{
		char out= input[++i];
		output += TranslitLetter(out);
	}
	return output;
}

std::string VariabilityName::TranslitLetter(char in) {
	std::string output = TRAL.translitAlfavit[in];
	if (output.empty())
		output = in;
	return output;
}

std::string VariabilityName::GetFIO()
{
	return _FIO;
}

std::string VariabilityName::GetIFO()
{
	return _IFO;
}

std::string VariabilityName::GetIOF()
{
	return _IOF;
}

std::string VariabilityName::GetFI()
{
	return _FI;
}

std::string VariabilityName::GetIF()
{
	return _IF;
}

std::string VariabilityName::GetFIO_t()
{
	return _FIO_t;
}

std::string VariabilityName::GetIFO_t()
{
	return _IFO_t;
}

std::string VariabilityName::GetIOF_t()
{
	return _IOF_t;
}

std::string VariabilityName::GetFI_t()
{
	return _FI_t;
}

std::string VariabilityName::GetIF_t()
{
	return _IF_t;
}

bool VariabilityName::isVarName()
{
	std::string firstName;
	std::string name;
	std::string lastName;
	int space = 0;
	for (char a : _FIO) {
		if (a == ' ') {
			space++;
			continue;
		}
		switch (space)
		{
		case (0):
			firstName += a;
			break;
		case (1):
			name += a;
			break;
		case (2):
			lastName += a;
			break;
		}
	}
	try {
		if (!varNames[name].empty()) {
			name = varNames[name];
			std::string str = firstName + " " + name + " " + lastName;
			VariabilityNameEng(str);
			return true;
		}
	}
	catch (const std::exception& e) {
		return false;
	}
	return false;
}
