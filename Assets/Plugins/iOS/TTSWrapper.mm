#import "TTSWrapper.h"

#import <AVFoundation/AVFoundation.h>

AVSpeechSynthesizer *synthesizer;

void _initTTS()
{
    synthesizer = [[AVSpeechSynthesizer alloc] init];
}

void _speak(const char * data, const char* lang,float rate)
{
    if(_isSpeaking())
        [synthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
    
    [[AVAudioSession sharedInstance] setCategory: AVAudioSessionCategoryPlayback  error: nil];
    
    AVSpeechUtterance *utterance = [AVSpeechUtterance speechUtteranceWithString:[NSString stringWithUTF8String:data]];
    
    NSInteger majorSystemVersion = [[[UIDevice currentDevice] systemVersion] integerValue];
    utterance.rate = rate;
//    if (majorSystemVersion == 8)
//        utterance.rate = 0.15;
//    else
//        utterance.rate = 0.53;
    
    utterance.voice = [AVSpeechSynthesisVoice voiceWithLanguage:[NSString stringWithUTF8String:lang]];
    [synthesizer speakUtterance:utterance];
}

bool _isSpeaking()
{
    return [synthesizer isSpeaking];
}

float _getDefaultRate(){
    
    NSInteger majorSystemVersion = [[[UIDevice currentDevice] systemVersion] integerValue];
    if (majorSystemVersion <= 8)
        return 0.15f;
    else
        return 0.53f;
}