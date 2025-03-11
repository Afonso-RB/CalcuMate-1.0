using System;
using System.Collections.Generic;
using System.Linq;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.util;
using Console = System.Console;

namespace CalcuMate_1._0
{
    public class NlpProcessor
    {
        private readonly StanfordCoreNLP _pipeline;

        public NlpProcessor()
        {
            var props = new Properties();
            props.setProperty("annotators", "tokenize,ssplit,pos,lemma,ner,parse");
            _pipeline = new StanfordCoreNLP(props);
        }

        public string ProcessInput(string input)
        {
            var annotation = new Annotation(input);
            _pipeline.annotate(annotation);

            var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList;
            if (sentences == null || sentences.size() == 0)
                return input;

            var sentence = sentences.get(0) as CoreMap;
            var tokens = sentence.get(typeof(CoreAnnotations.TokensAnnotation)) as ArrayList;

            var processedInput = string.Join(" ", tokens.Cast<CoreLabel>().Select(token => token.lemma()));
            return processedInput;
        }
    }
}
