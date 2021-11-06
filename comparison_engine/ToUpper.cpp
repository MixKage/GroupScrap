#include "ToUpper.h"
#include <map>
#include <string>
#include <fstream>
#include <iostream>

ToUpper::ToUpper() {
	std::ifstream in("toUpper.txt");
	std::string s;
	char rus;
	std::string eng;

	if (in.is_open())
	{
		while (!in.eof())
		{
			//CLEAR
			std::getline(in, s);
			rus = s[1];
			for (size_t i = 0; i < s.size(); i++)
				if (s[i] != '>')
					continue;
				else {
					s.erase(0, i + 1);
					break;
				}
			eng = s;

			toUpperAlfavit.insert(std::pair<char, std::string>(rus, eng));
		}
	}
	else {
		std::cout << "ToUpper can't open! ToUpper not work!" << std::endl;
	}
	in.close();
}