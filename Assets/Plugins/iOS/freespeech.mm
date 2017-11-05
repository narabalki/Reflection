#import "FreeSpeech.h"

static predict_next_helper* engine;

//You need to return a copy of the c string so that Unity handles the memory and gets a valid value.

char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

extern "C"
{
    void _initFS(char *cPath)
    {
        string path = cPath;
        engine = new predict_next_helper("en",path);
    }
    
    const char * _createSentence( char *cString, bool useUW)
    {
        string temp = cString;
        string str = engine->createSentence("en", temp, useUW);
        return cStringCopy(str.c_str());
    }

    const char * _createZhSentence( char *cString, bool useUW)
    {
        string temp = cString;
        string str = engine->createSentence("zh", temp, useUW);

        return cStringCopy(str.c_str());
    }
    
    const char * _createZhpSentence( char *cString, bool useUW)
    {
        string temp = cString;
        string str = engine->createSentence("zhp", temp, useUW);
        
        return cStringCopy(str.c_str());
    }
    
    const char * _getWord(char *cString)
    {
        string temp = cString;
        string str = engine->deconvert_en_word("en", temp);
        return cStringCopy(str.c_str());
    }

    const char * _getZhWord(char *cString)
    {
        string temp = cString;
        string str = engine->deconvert_en_word("zh", temp);
        return cStringCopy(str.c_str());
    }
    
    const char * _getZhpWord(char *cString)
    {
        string temp = cString;
        string str = engine->deconvert_en_word("zhp", temp);
        return cStringCopy(str.c_str());
    }
    
    const char * _getValidQuestions(char *unl_cString, char *uw_cString, char *ques_cString)
    {
        string unl  = unl_cString;
        string uw  = uw_cString;
        string ques  = ques_cString;
        string str = engine->getValidQuestions("en",unl,uw,ques);
        return cStringCopy(str.c_str());
    }
    
    const char * _getValidRelations(char *unl_cString, char *uw_cString)
    {
        string unl  = unl_cString;
        string uw  = uw_cString;
        string str = engine->predictrels2("en",unl,uw);
        return cStringCopy(str.c_str());
    }
    
    const char * _getPredictions(char *unl_cString)
    {
        string unl  = unl_cString;
        string str = engine->predictrels_next("en",unl);
        return cStringCopy(str.c_str());
    }
    
    const bool _addNewNoun(char *noun_details)
    {
        string noun = noun_details;
        bool res = engine->addNewNoun("en",noun);
        return res;
    }
    
    const bool _deleteWord(char *wordId)
    {
        string word = wordId;
        bool res = engine->deleteWord("en",word);
        return res;
    }
    
    const bool _isValidSentence(char *unl_cString)
    {
        string unl  = unl_cString;
        bool res = engine->isValidSentence("en",unl);
        return res;
    }
}
