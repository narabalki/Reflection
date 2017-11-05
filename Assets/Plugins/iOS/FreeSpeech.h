//
//  main.h
//  FreeSpeech
//
//  Created by Abu Saad Papa on 26/06/13.
//  Copyright (c) 2013 Abu Saad Papa. All rights reserved.
//

#ifndef FreeSpeech_FreeSpeech_h
#define FreeSpeech_FreeSpeech_h
#include <iostream>

using namespace std;

char* cStringCopy(const char* string);

class predict_next_helper{
public:
    predict_next_helper(string lang, string path);
    
    string createSentence(string lang, string unl, bool def = false);
    
    string deconvert_en_word(string lang, string uw, bool def = false);
    
    string predictrels2(string lang, string forest1, string forest2);
    
    string getValidQuestions(string lang, string unl, string uw, string questions);
    
    string predictrels_next(string lang, string forest1);
    
    bool addNewNoun(string lang, string noun_details);
    
    bool deleteWord(string lang, string wordId);
    
    bool isValidSentence(string lang, string unl);
};


#endif
