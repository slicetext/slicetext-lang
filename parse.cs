namespace Parse
{
    using TokenType=Lang.Program.TokenType;
    class Parser
    {
        private int i=0;
        private int line=0;
        public void parse(List<TokenType> Tokens,List<object> Types)
        {
            for (i=0;i<Tokens.Count;i++)
            {
                TokenType current=Tokens[i];
                TokenType lookahead=Tokens[i+1];
                if(current==TokenType.NEWLINE)
                {
                    line++;
                }
            }
        }
        void eat(TokenType expected,TokenType lookahead)
        {
            if(lookahead==expected)
            {
                i++;
            }
            else
            {
                error(line,"Expected "+expected+", instead got "+lookahead);
            }
        }
        void error(int line,string message)
        {
            Console.WriteLine("Line "+line+": "+message);
        }
    }
}