using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;

class Program 
{
    //Azure AI Neural TTS offers over 440 neural voices across over 140 languages and locales. 
    static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                Console.WriteLine($"Speech synthesized for text: [{text}]");
                break;
            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
            default:
                break;
        }
    }

    async static Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Access values using IConfiguration
        string apiKey = configuration["SpeechKey"];
        string apiRegion = configuration["SpeechRegion"];
        var speechConfig = SpeechConfig.FromSubscription(apiKey, apiRegion);      

        // The language of the voice that speaks.
        speechConfig.SpeechSynthesisVoiceName = "en-AU-WilliamNeural"; 

        using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
        {
            // Get text from the console and synthesize to the default speaker.
            Console.WriteLine("Enter some text that you want to speak >");
            string text = @"<speak xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts' xmlns:emo='http://www.w3.org/2009/10/emotionml' version='1.0' xml:lang='en-US'><voice name='en-US-NancyNeural'><s /><mstts:express-as style='shouting'>'We are friends now, I don't understand why you don't discuss your plans!' ,</mstts:express-as><s /><mstts:express-as style='Default'><prosody contour='(0%, -36%) (44%, -36%) (65%, -4%) (77%, +57%) (85%, +0%)'>I shouted.</prosody></mstts:express-as></voice> <voice name='en-US-DavisNeural'><s /><mstts:express-as style='unfriendly'>'<prosody contour='(4%, -1%) (49%, +0%) (76%, +49%) (83%, -27%)'>I needed to make certain</prosody><prosody contour='(63%, -5%) (76%, +54%) (85%, -4%)'> you didn't attempt to harm me.</prosody>'</mstts:express-as></voice> <voice name='en-US-NancyNeural'><s /><mstts:express-as style='Default'><prosody contour='(45%, -2%) (51%, +37%) (59%, -1%) (75%, -21%) (91%, +18%)'>I noticed he said 'attempt' and not 'intend',</prosody> I had no reason to harm him.</mstts:express-as><s /></voice> <voice name='en-US-NancyNeural'><s /><mstts:express-as style='terrified'><prosody rate='-15.00%'>'I just wanted to help you, </prosody><prosody rate='-15.00%' contour='(8%, -4%) (18%, +4%) (28%, -30%) (82%, +43%) (94%, -11%)'>otherwise how can we escape from this planet?'</prosody></mstts:express-as><s /></voice> <voice name='en-US-DavisNeural'><s /><mstts:express-as style='sad'>'I don't have a chance to think about it, <prosody contour='(39%, -1%) (44%, -21%) (76%, -23%) (93%, +37%)'>leave me alone please.</prosody>'</mstts:express-as><s /></voice> <voice name='en-US-NancyNeural'><s /><mstts:express-as style='Default'><prosody contour='(19%, +3%) (58%, +3%) (67%, -23%) (95%, +23%)'>he said.</prosody></mstts:express-as><s /> <s /><mstts:express-as style='Default'>I didn't answer, and I turned around and closed the door.</mstts:express-as><s /></voice></speak>";
                        // string text = Console.ReadLine();

            var speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(text);
            OutputSpeechSynthesisResult(speechSynthesisResult, text);
        }
    }
}