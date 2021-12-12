using DeeplTranslator = Deepl.Deepl; 
using Language = Deepl.Deepl.Language;

class Program 
{
    static void Main(string[] args)
    {
        var translator = new DeeplTranslator(selectedLanguage: Language.FR, targetLanguage: Language.IT, "j'adore nager même");

        Console.WriteLine(translator.Resp);
    }
}