namespace Parse
{
    using TokenType=Lang.Program.TokenType;
    class Parser
    {
        private int i=0;
        private int line=0;
        private TokenType lookahead;
        private List<string> ast= new List<string>();
        public void parse(List<TokenType> Tokens,List<object> Types)
        {
            for (i=0;i<Tokens.Count;i++)
            {
                TokenType current=Tokens[i];
                if(i<Tokens.Count-1)
                {
                    lookahead=Tokens[i+1];
                }
                if(current==TokenType.NEWLINE)
                {
                    line++;
                }
                if(n_eat(new List<TokenType>{TokenType.NUMBER},Tokens[i]))
                {
                    ast.Add(Tokens[i].ToString());
                    i++;
                    while(n_eat(new List<TokenType>{TokenType.NUMBER,TokenType.PLUS},Tokens[i]))
                    {
                        if(Tokens[i]==TokenType.PLUS)
                        {
                        ast.Add(Tokens[i].ToString());
                        }
                        else
                        {
                            if(Types[i]!=null)
                            {
                                #pragma warning disable CS8604
                                ast.Add(Types[i]?.ToString());
                                #pragma warning restore CS8604
                            }
                        }
                        i++;
                    }
                }
            }
            Console.WriteLine(string.Join("",ast));
        }
        bool eat(List<TokenType> expected,TokenType lookahead)
        {
            foreach(TokenType j in expected)
            {
                if(lookahead==j)
                {
                    return true;
                }
            }
            error(line,"Expected "+string.Join(", ",expected)+", instead got "+lookahead);
            return false;
        }
        bool n_eat(List<TokenType> expected,TokenType lookahead)
        {
            foreach(TokenType j in expected)
            {
                if(lookahead==j)
                {
                    return true;
                }
            }
            return false;
        }
        void error(int line,string message)
        {
            Console.WriteLine("Line "+line+": "+message);
        }
    }
}