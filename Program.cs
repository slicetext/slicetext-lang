using Expr;
namespace Lang
{
    class Program
    {
        public bool hadError=false;
        public enum TokenType
        {
            L_PAREN, R_PAREN, L_BRACE, R_BRACE, COMMA, DOT, MINUS,
            PLUS, SEMICOLON, SLASH, STAR,
            //MULTI CHAR
            BANG, BANG_EQUAL, EQUAL, EQUAL_EQUAL, GREATER, GREATER_EQUAL, LESS,
            LESS_EQUAL,
            //LITERALS
            IDENTIFIER, STRING, NUMBER,
            //KEYWORDS
            AND,CLASS,ELSE,FALSE,FUNC,FOR,IF,NULL,OR,PRINT,RETURN,SUPER,TRUE,WHILE,VAR,THIS,
            EOF
        }
        public static void Main(string[] args)
        {
            var program=new Program();
            program.run();
        }
        public void run()
        {
            while(true)
            {
                Console.Write(">");
                var a=Console.ReadLine();
                if(a!=null)
                {
                    var program=new Program();
                    var a2=program.lex(a);
                    var a3=new List<TokenType>();
                    foreach(TokenType t in a2[0])
                    {
                        a3.Add(t);
                    }
                }
            }
        }
        public List<List<object>> lex(string code)
        {
            List<object> tokens=new List<object>();
            List<object> types=new List<object>();
            int line=1;
            int n=0;
            var i=0;
            while(i<code.ToCharArray().Length)
            {
                char current=code.ToCharArray()[i];
                switch(current)
                {
                    case '(':tokens.Add(TokenType.L_PAREN);types.Add("null"); break;
                    case ')':tokens.Add(TokenType.R_PAREN);types.Add("null"); break;
                    case '{':tokens.Add(TokenType.L_BRACE);types.Add("null"); break;
                    case '}':tokens.Add(TokenType.R_BRACE);types.Add("null"); break;
                    case ',':tokens.Add(TokenType.COMMA);types.Add("null"); break;
                    case '.':tokens.Add(TokenType.DOT);types.Add("null"); break;
                    case '-':tokens.Add(TokenType.MINUS);types.Add("null"); break;
                    case '+':tokens.Add(TokenType.PLUS);types.Add("null"); break;
                    case ';':tokens.Add(TokenType.SEMICOLON);types.Add("null"); break;
                    case '*':tokens.Add(TokenType.STAR);types.Add("null"); break;
                    case '&':addAnd();break;
                    case '|':addOr();break;
                    case '!':if(ahead('=')==true)tokens.Add(TokenType.BANG_EQUAL);else tokens.Add(TokenType.BANG);types.Add("null");break;
                    case '=':if(ahead('=')==true)tokens.Add(TokenType.EQUAL_EQUAL);else tokens.Add(TokenType.EQUAL);types.Add("null");break;
                    case '<':if(ahead('=')==true)tokens.Add(TokenType.LESS_EQUAL);else tokens.Add(TokenType.LESS);types.Add("null");break;
                    case '>':if(ahead('=')==true)tokens.Add(TokenType.GREATER_EQUAL);else tokens.Add(TokenType.GREATER);types.Add("null");break;
                    case '/':if(ahead('/')==true)
                        {
                            var ls=i+1;
                            while(i!=code.ToCharArray().Length && ls!='\n')
                            {
                                ls++;i++;
                            }
                        }
                        else
                        {
                            tokens.Add(TokenType.SLASH);
                            types.Add("null");
                        }
                        break;
                    //string
                    case '"':stringer();break;
                    case ' ':
                    case '\r':
                    case '\t':
                        // Ignore whitespace.
                        break;
                    case '\n':line++; break;
                    default: if(Char.IsDigit(current))
                    {
                    number();}
                    else if(Char.IsLetter(current)){identifier();}
                    else{ error(line,"Unexpected Character "+current);} break;
                }
                i++;
            }
            bool ahead(char expected)
            {
                var l=i+1;
                if(code.ToCharArray().Length<=l)
                {
                    return false;
                }
                if(code[l]==expected)
                {
                    i++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            void stringer()
            {
                var start=i;
                var l=i+1;
                while(code[l]!='"'&&i!=code.ToCharArray().Length)
                {
                    if(code[i]=='\n')line++;
                    i++;
                    l=i+1;
                }
                if(i>=code.ToCharArray().Length)
                {
                    error(line,"Unterminated String");
                }
                i++;
                string value=code.Substring(start+1,i-1);
                tokens.Add(TokenType.STRING);
                types.Add(value);
            }
            void number()
            {
                var l=i+1;
                var l2=i+2;
                var oi=i;
                // if(code.ToCharArray().Length>2)
                // {
                // while(Char.IsDigit(code[l])) i++;l++;l2++;Console.WriteLine("Loop1");
                // if(code[l]=='.'&&Char.IsDigit(code[l2]))
                // {
                //     i++;
                //     l++;
                //     l2++;
                //     while(Char.IsDigit(code[l])) i++;l=i+1;l2=i+2;Console.WriteLine("Loop2");
                // }
                // }
                // tokens.Add(TokenType.NUMBER);
                // string value=code.Substring(code[oi],code[i]);
                // Console.WriteLine(value);
                // types.Add(Convert.ToDouble(value.ToString()));
                while (l<=code.ToCharArray().Length-1 && Char.IsDigit(code[l]))
                {
                    i+=1;
                    l+=1;
                    l2+=1;
                }
                // Look for a fractional part.
                if (l2<=code.ToCharArray().Length-1 && code[l] == '.' && Char.IsDigit(code[l2])) 
                {
                    // Consume the "."
                    i++;l++;l2++;
                    while (l<=code.ToCharArray().Length-1 && Char.IsDigit(code[l]))
                    {
                        i=i+1;
                        l=l+1;
                        l2=l2+1;
                    }
                }
                tokens.Add(TokenType.NUMBER);
                string value=code.Substring(oi,(i-oi)+1);
                types.Add(Convert.ToDouble(value.ToString()));
        
            }
            void addAnd()
            {
                if(ahead('&')==true){tokens.Add(TokenType.AND);
                types.Add("null");}
                else {error(line,"Unexpected Character "+code[i]);}
            }
            void addOr()
            {
                if(ahead('|')==true){tokens.Add(TokenType.OR);
                types.Add("null");}
                else {error(line,"Unexpected Character "+code[i]);}
            }
            void identifier()
            {
                var oi=i;
                var l=i+1;
                while(l<=code.ToCharArray().Length-1 && char.IsLetterOrDigit(code[l]))
                {
                    l=l+1;
                    i=i+1;
                }
                
                Dictionary<string,TokenType>reserved=new Dictionary<string,TokenType>();
                reserved.Add("and",TokenType.AND);
                reserved.Add("class",TokenType.CLASS);
                reserved.Add("else",TokenType.ELSE);
                reserved.Add("false",TokenType.FALSE);
                reserved.Add("for",TokenType.FOR);
                reserved.Add("func",TokenType.FUNC);
                reserved.Add("if",TokenType.IF);
                reserved.Add("null",TokenType.NULL);
                reserved.Add("or",TokenType.OR);
                reserved.Add("print",TokenType.PRINT);
                reserved.Add("return",TokenType.RETURN);
                reserved.Add("super",TokenType.SUPER);
                reserved.Add("this",TokenType.THIS);
                reserved.Add("true",TokenType.TRUE);
                reserved.Add("var",TokenType.VAR);
                reserved.Add("while",TokenType.WHILE);

                string text=code.Substring(oi,(i-oi)+1);
                if(reserved.ContainsKey(text))
                {
                    tokens.Add(reserved[text]);
                    types.Add("null");
                }
                else
                {
                    tokens.Add(TokenType.IDENTIFIER);
                    types.Add(text);
                }
            }
                n++;
            
            return new List<List<object>>{tokens,types};
        }
        public void error(int line, string message)
        {
            Console.WriteLine("Line "+line+": "+message);
            hadError=true;
        }
    }
}