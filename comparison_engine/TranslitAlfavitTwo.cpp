#include "TranslitAlfavitTwo.h"
#include <map>
#include <string>
#include <fstream>
#include <iostream>

TranslitAlfavitTwo::TranslitAlfavitTwo() {
	std::ifstream in("alfavitTranslit.txt");
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

			translitAlfavit.insert(std::pair<char, std::string>(rus, eng));
		}
	}
	else {
		std::cout << "Alfavit can't open! Translit not work!" << std::endl;
	}
	in.close();
}