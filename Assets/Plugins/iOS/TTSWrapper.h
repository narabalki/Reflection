extern "C" void _initTTS();

extern "C" void _speak(const char * data, const char* lang,float rate);

extern "C" bool _isSpeaking();

extern "C" float _getDefaultRate();