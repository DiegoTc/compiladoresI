using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;
using Project_Compiladores1.Arbol;
using Project_Compiladores1.Semantico;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser : Parser
    {

        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }

        public Sentencia parse()
        {
            try
            {
                return StatementList();
            }
            catch (Exception ex) { throw ex; }
        }

        Sentencia StatementList()
        {
            try
            {
                Sentencia ret = Statement();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                    ret.sig = StatementList();
                }
                return ret;
            }
            catch (Exception ex) { throw ex; }
        }

        Sentencia Statement()
        {
            try
            {
                switch (currentToken.Tipo)
                {
                    case TipoToken.TK_IF:
                        currentToken = lex.NextToken();
                        return parseIf();

                    case TipoToken.TK_FOR:
                        currentToken = lex.NextToken();
                        return parseFor();

                    case TipoToken.TK_WHILE:
                        currentToken = lex.NextToken();
                        return parseWhile();

                    case TipoToken.TK_CASE:
                        currentToken = lex.NextToken();
                        return parseSwitch();

                    case TipoToken.TK_REPEAT:
                        currentToken = lex.NextToken();
                        return parseDo();

                    #region variable
                    case TipoToken.TK_VAR:
                        {
                            currentToken = lex.NextToken();
                            Declaracion ret = VariableDeclarationList();
                            if (currentToken.Tipo != TipoToken.TK_END)
                                throw new Exception("Se esperaba end.");
                            else
                            {
                                currentToken = lex.NextToken();
                                return ret;
                            }
                        }
                    #endregion

                    case TipoToken.TK_ID:
                        return parseAssignOrCall();

                    #region read/print
                    case TipoToken.TK_PRINT:
                        {
                            currentToken = lex.NextToken();
                            S_Print ret = new S_Print();
                            ret.Expr = Expr();
                            return ret;
                        }
                    case TipoToken.TK_READ:
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo != TipoToken.TK_ID)
                                throw new Exception("Se esperaba identificador.");
                            else
                            {
                                S_Read ret = new S_Read();
                                ret.var = new Variable(currentToken.Lexema, AccessList(currentToken.Lexema));
                                return ret;
                            }
                        }
                    #endregion

                    #region Procedure
                    case TipoToken.TK_VOID:
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo != TipoToken.TK_ID)
                                throw new Exception("Se esperaba ID");
                            else
                            {
                                S_Functions ret = new S_Functions();
                                ret.Retorno = new Voids();
                                ret.Var = currentToken.Lexema;
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo != TipoToken.TK_OPENPAR)
                                    throw new Exception("Se esperaba \"(\"");
                                else
                                {
                                    currentToken = lex.NextToken();
                                    ret.Campo = VariableDeclarationList();
                                    if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                                        throw new Exception("Se esperaba\")\"");
                                    else
                                    {
                                        currentToken = lex.NextToken();
                                        ret.S = CodeBlock();
                                        return ret;
                                    }
                                }
                            }
                        }
                    #endregion

                    #region Function
                    case TipoToken.TK_FUNCTION:
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo != TipoToken.TK_ID)
                                throw new Exception("Se esperaba ID");
                            else
                            {
                                S_Functions ret = new S_Functions();
                                ret.Var = currentToken.Lexema;
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo != TipoToken.TK_OPENPAR)
                                    throw new Exception("Se esperaba \"(\"");
                                else
                                {
                                    currentToken = lex.NextToken();
                                    ret.Campo = VariableDeclarationList();
                                    if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                                        throw new Exception("Se esperaba\")\"");
                                    else
                                    {
                                        currentToken = lex.NextToken();
                                        if (currentToken.Tipo != TipoToken.TK_DOSPUNTOS)
                                            throw new Exception("Se esperaba\":\"");
                                        else
                                        {
                                            currentToken = lex.NextToken();
                                            ret.Retorno = ParseType();
                                            ret.S = CodeBlock();
                                            return ret;
                                        }
                                    }
                                }
                            }
                        }
                    #endregion

                    #region type
                    case TipoToken.TK_TYPE:
                        {
                            currentToken = lex.NextToken();
                            TypeDef ret = TypeDeclarationList();
                            if (currentToken.Tipo != TipoToken.TK_END)
                                throw new Exception("Se esperaba end.");
                            else
                            {
                                currentToken = lex.NextToken();
                                return ret;
                            }
                        }
                    #endregion

                    #region break/continue/return
                    case TipoToken.TK_BREAK:
                        currentToken = lex.NextToken();
                        return new S_Break();

                    case TipoToken.TK_CONTINUE:
                        currentToken = lex.NextToken();
                        return new S_Continue();

                    case TipoToken.TK_RETURN:
                        {
                            currentToken = lex.NextToken();
                            S_Return ret = new S_Return();
                            ret.Expr = Expr();
                            return ret;
                        }
                    #endregion
                    default:
                        throw new Exception("Sentencia no reconocida.");
                }
            }
            catch (Exception ex) { throw ex; }
        }

        Sentencia CodeBlock()
        {
            if(currentToken.Tipo!=TipoToken.TK_BEGIN)
                throw new Exception("Se esperaba Begin");
            else
            {
                currentToken=lex.NextToken();
                Sentencia ret;
                try
                {
                    ret = CompoundStatementList();
                }
                catch (Exception ex) { throw ex; }
                if(currentToken.Tipo!=TipoToken.TK_END)
                    throw new Exception("Se esperaba end");
                else
                {
                    currentToken=lex.NextToken();
                    return ret;
                }
            }
        }

        Sentencia CompoundStatementList()
        {
            try
            {
                Sentencia ret = CompoundStatement();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                    ret.sig = CompoundStatementList();
                }
                return ret;
            }
            catch (Exception ex) { throw ex; }
        }

        Sentencia CompoundStatement()
        {
            try
            {
                switch (currentToken.Tipo)
                {
                    case TipoToken.TK_IF:
                        currentToken = lex.NextToken();
                        return parseIf();

                    case TipoToken.TK_FOR:
                        currentToken = lex.NextToken();
                        return parseFor();

                    case TipoToken.TK_WHILE:
                        currentToken = lex.NextToken();
                        return parseWhile();

                    case TipoToken.TK_CASE:
                        currentToken = lex.NextToken();
                        return parseSwitch();

                    case TipoToken.TK_REPEAT:
                        currentToken = lex.NextToken();
                        return parseDo();

                    case TipoToken.TK_ID:
                        return parseAssignOrCall();

                    #region read/print/break/continue/return
                    case TipoToken.TK_PRINT:
                        {
                            currentToken = lex.NextToken();
                            S_Print ret = new S_Print();
                            ret.Expr = Expr();
                            return ret;
                        }
                    case TipoToken.TK_READ:
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo != TipoToken.TK_ID)
                                throw new Exception("Se esperaba identificador.");
                            else
                            {
                                S_Read ret = new S_Read();
                                ret.var = new Variable(currentToken.Lexema, AccessList(currentToken.Lexema));
                                return ret;
                            }
                        }

                    case TipoToken.TK_BREAK:
                        currentToken = lex.NextToken();
                        return new S_Break();

                    case TipoToken.TK_CONTINUE:
                        currentToken = lex.NextToken();
                        return new S_Continue();

                    case TipoToken.TK_RETURN:
                        {
                            currentToken = lex.NextToken();
                            S_Return ret = new S_Return();
                            ret.Expr = Expr();
                            return ret;
                        }
                    #endregion
                    default:
                        throw new Exception("Sentencia no reconocida.");
                }
            }
            catch (Exception ex) { throw ex; }
        }

        S_If parseIf()
        {
            S_If ret = new S_If();
            try
            {
                ret.Condicion = Expr();
            }
            catch (Exception ex) { throw ex; }
            if (currentToken.Tipo != TipoToken.TK_THEN)
                throw new Exception("Se esperaba then");
            else
            {
                currentToken = lex.NextToken();
                try
                {
                    ret.Cierto = CodeBlock();
                }
                catch (Exception ex) { throw ex; }
                if (currentToken.Tipo == TipoToken.TK_ELSE)
                {
                    currentToken = lex.NextToken();
                    try
                    {
                        ret.Falso = CodeBlock();
                    }
                    catch (Exception ex) { throw ex; }
                }
                return ret;
            }
        }

        S_For parseFor()
        {
            S_For ret = new S_For();
            ret.Tip = new Entero();
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Se esperaba identificador.");
            else
            {
                ret.Var = new Variable(currentToken.Lexema, null);
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_ASSIGN)
                    throw new Exception("Se esperaba el operador de asignacion");
                else
                {
                    currentToken = lex.NextToken();
                    try
                    {
                        ret.Inicio = Expr();
                    }
                    catch (Exception ex) { throw ex; }
                    switch (currentToken.Tipo)
                    {
                        case TipoToken.TK_TO:
                            currentToken = lex.NextToken();
                            MenorQue cond;
                            try
                            {
                                cond = new MenorQue(ret.Var, Expr());
                            }
                            catch (Exception ex) { throw ex; }
                            ExpMasMas it = new ExpMasMas();
                            it.ID = ret.Var;
                            ret.Condicion = cond;
                            ret.Iteracion = it;
                            break;
                        case TipoToken.TK_DOWNTO:
                            currentToken = lex.NextToken();
                            MayorQue cond1;
                            try
                            {
                                cond1 = new MayorQue(ret.Var, Expr());
                            }
                            catch (Exception ex) { throw ex; }
                            ExpMenosMenos it1 = new ExpMenosMenos();
                            it1.ID = ret.Var;
                            ret.Condicion = cond1;
                            ret.Iteracion = it1;
                            break;
                        default:
                            throw new Exception("Se esperaba to/Downto");
                    }
                    if (currentToken.Tipo != TipoToken.TK_DO)
                        throw new Exception("Se esperaba do.");
                    else
                    {
                        currentToken = lex.NextToken();
                        try
                        {
                            ret.S = CodeBlock();
                        }
                        catch (Exception ex) { throw ex; }
                        return ret;
                    }
                }
            }
        }

        S_While parseWhile()
        {
            S_While ret = new S_While();
            try
            {
                ret.Condicion = Expr();
            }
            catch (Exception ex) { throw ex; }
            if (currentToken.Tipo != TipoToken.TK_DO)
                throw new Exception("Se esperaba do.");
            else
            {
                currentToken = lex.NextToken();
                try
                {
                    ret.S = CodeBlock();
                }
                catch (Exception ex) { throw ex; }
                return ret;
            }
        }

        S_Do parseDo()
        {
            S_Do ret = new S_Do();
            try
            {
                ret.S = CodeBlock();
            }
            catch (Exception ex) { throw ex; }
            if (currentToken.Tipo != TipoToken.TK_UNTIL)
                throw new Exception("Se esperaba until.");
            else
            {
                currentToken = lex.NextToken();
                try
                {
                    ret.Condicion = Expr();
                }
                catch (Exception ex) { throw ex; }
                return ret;
            }
        }

        S_Switch parseSwitch()
        {
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Se esperaba identificador.");
            else
            {
                string tmp = currentToken.Lexema;
                currentToken = lex.NextToken();
                S_Switch ret = new S_Switch();
                try { ret.Var = new Variable(tmp, AccessList(tmp)); }
                catch (Exception ex) { throw ex; }
                if (currentToken.Tipo != TipoToken.TK_OF)
                    throw new Exception("Se esperaba of.");
                else
                {
                    currentToken = lex.NextToken();
                    try { ret.Casos = caseList(); }
                    catch (Exception ex) { throw ex; }
                    if (currentToken.Tipo == TipoToken.TK_ELSE)
                    {
                        currentToken = lex.NextToken();
                        try { ret.sdefault = CodeBlock(); }
                        catch (Exception ex) { throw ex; }
                    }
                    return ret;
                }
            }
        }

        Cases caseList()
        {
            Cases ret = parseCase();
            while (currentToken.Tipo == TipoToken.TK_INT_LIT)
            {
                ret.Sig = caseList();
            }
            return ret;
        }

        Cases parseCase()
        {
            if (currentToken.Tipo != TipoToken.TK_INT_LIT)
                throw new Exception("Se esperaba una constante numérica.");
            else
            {
                Cases ret = new Cases();
                ret.Valor = new LiteralEntero(int.Parse(currentToken.Lexema));
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_DOSPUNTOS)
                    throw new Exception("se esperaba :");
                else
                {
                    currentToken = lex.NextToken();
                    try { ret.S = CodeBlock(); }
                    catch (Exception ex) { throw ex; }
                    return ret;
                }
            }
        }

        Sentencia parseAssignOrCall()//Asume que no se ha consumido el ID
        {
            string tmp = currentToken.Lexema;
            currentToken = lex.NextToken();
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_PUNTO:
                case TipoToken.TK_OPENCOR:
                    {
                        S_Asignacion ret = new S_Asignacion();
                        try
                        {
                            ret.id = new Variable(tmp, AccessList(tmp));
                        }
                        catch (Exception ex) { throw ex; }
                        if (currentToken.Tipo != TipoToken.TK_ASSIGN)
                            throw new Exception("Se esperaba Asignacion.");
                        else
                        {
                            currentToken = lex.NextToken();
                            try
                            {
                                ret.Valor = Expr();
                            }
                            catch (Exception ex) { throw ex; }
                            return ret;
                        }
                    }
                case TipoToken.TK_OPENPAR:
                    {
                        currentToken = lex.NextToken();
                        S_LlamadaFunc ret = new S_LlamadaFunc();
                        ret.Var = new Variable(tmp, null);
                        ret.Variables = new ListaExpre();
                        try
                        {
                            ret.Variables.Ex = ExprList();
                        }
                        catch (Exception ex) { throw ex; }
                        if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                            throw new Exception("Se esperaba ).");
                        else
                        {
                            currentToken = lex.NextToken();
                            return ret;
                        }
                    }
                default:
                    throw new Exception("Sentencia no reconocida.");
            }
        }

        Declaracion VariableDeclarationList()
        {
            try
            {
                Declaracion ret = VariableDeclaration();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                    ret.Sig = VariableDeclarationList();
                }
                return ret;
            }
            catch (Exception ex) { throw ex; }
        }

        Declaracion VariableDeclaration()
        {
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Se esperaba identificador.");
            else
            {
                Declaracion ret = new Declaracion();
                ret.Var = new Variable(currentToken.Lexema, null);
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba :.");
                else
                {
                    currentToken = lex.NextToken();
                    try
                    {
                        ret.Tip = ParseType();
                    }
                    catch (Exception ex) { throw ex; }
                    return ret;
                }
            }
        }

        TypeDef TypeDeclarationList()
        {
            try
            {
                TypeDef ret = TypeDeclaration();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                    ret.Sig = TypeDeclarationList();
                }
                return ret;
            }
            catch (Exception ex) { throw ex; }
        }

        TypeDef TypeDeclaration()
        {
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Se esperaba identificador.");
            else
            {//type id
                string tmp = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_ASSIGN)
                    throw new Exception("Se esperaba asignacion.");
                else
                {//type id :=
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_RECORD)
                    {//type id := record
                        currentToken = lex.NextToken();
                        Structs ret = new Structs();
                        ret.nombre = tmp;
                        try
                        {
                            ret.campos = VariableDeclarationList();
                        }
                        catch (Exception ex) { throw ex; }
                        if (currentToken.Tipo != TipoToken.TK_END)
                            throw new Exception("Se esperaba end.");
                        else
                        {
                            currentToken = lex.NextToken();
                            Struct type = new Struct();
                            type.Campos = new T_Campos();
                            type.nombre = ret.nombre;
                            Declaracion temp = ret.campos;
                            while (temp != null)
                            {
                                type.Campos.Add(temp.Var.id, temp.Tip);
                                temp = temp.Sig;
                            }
                            try { InfSemantica.getInstance().tblTipos.Add(type.nombre, type); }
                            catch (Exception ex) { throw new Exception("Ya se ha definido un tipo con ese nombre."); }
                            return ret;
                        }
                    }
                    else
                    {//type id := something
                        Alias ret = new Alias();
                        ret.type = new UserType();
                        ret.type.Nombre = tmp;
                        try
                        {
                            ret.type.Tip = ParseType();
                        }
                        catch (Exception ex) { throw ex; }
                        InfSemantica.getInstance().tblTipos.Add(ret.type.Nombre, ret.type);
                        return ret;
                    }
                }
            }
        }

        Access AccessList(string par)
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_PUNTO:
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_ID)
                            throw new Exception("Se esperaba identificador.");
                        else
                        {
                            string tmp = currentToken.Lexema;
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_OPENCOR)
                            {
                                Access ret;
                                try
                                {
                                    ret = AccessList(tmp);
                                    ret.Next = AccessList(null);
                                }
                                catch (Exception ex) { throw ex; }
                                return ret;
                            }
                            else
                            {
                                AccessMiembro ret = new AccessMiembro();
                                ret.Id = tmp;
                                try { ret.Next = AccessList(null); }
                                catch (Exception ex) { throw ex; }
                                return ret;
                            }
                        }
                    }

                case TipoToken.TK_OPENCOR:
                    {
                        if (par == null)
                            throw new Exception("Se esperaba otro accesor.");
                        currentToken = lex.NextToken();
                        AccessArreglo ret = new AccessArreglo();
                        ret.nombre = par;
                        try { ret.Cont = ExprList(); }
                        catch (Exception ex) { throw ex; }
                        if (currentToken.Tipo != TipoToken.TK_CLOSECOR)
                            throw new Exception("Se esperaba ].");
                        else
                        {
                            currentToken = lex.NextToken();
                            try { ret.Next = AccessList(null); }
                            catch (Exception ex) { throw ex; }
                            return ret;
                        }
                    }

                default: return null;
            }
        }

        ArrayList ExprList()
        {
            ArrayList ret = new ArrayList();
            try
            {
                ret.Add(Expr());
            }
            catch (Exception ex) { throw ex; }
            while (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                try
                {
                    ret.Add(Expr());
                }
                catch (Exception ex) { throw ex; }
            }
            return ret;
        }

        Tipo ParseType()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT:
                    {
                        currentToken = lex.NextToken();
                        return new Entero();
                    }
                case TipoToken.TK_BOOL:
                    {
                        currentToken = lex.NextToken();
                        return new Booleano();
                    }
                case TipoToken.TK_CHAR:
                    {
                        currentToken = lex.NextToken();
                        return new Caracter();
                    }
                case TipoToken.TK_ID:
                    {
                        if (InfSemantica.getInstance().tblTipos.ContainsKey(currentToken.Lexema))
                        {
                            string nom = currentToken.Lexema;
                            currentToken = lex.NextToken();
                            return InfSemantica.getInstance().tblTipos[nom];
                        }
                        else throw new Exception("Tipo no reconocido.");
                    }
                case TipoToken.TK_ARRAY:
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_OPENCOR)
                            throw new Exception("Se esperaba [");
                        else
                        {
                            currentToken = lex.NextToken();
                            Arreglo ret = new Arreglo();
                            try
                            {
                                ret.Rangos = ExprList();
                            }
                            catch (Exception ex) { throw ex; }
                            ret.Dimensiones = ret.Rangos.Count;
                            if (currentToken.Tipo != TipoToken.TK_CLOSECOR)
                                throw new Exception("Se esperaba ]");
                            else
                            {
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo != TipoToken.TK_OF)
                                    throw new Exception("Se esperaba of.");
                                else
                                {
                                    currentToken = lex.NextToken();
                                    try
                                    {
                                        ret.Contenido = ParseType();
                                    }
                                    catch (Exception ex) { throw ex; }
                                    return ret;
                                }
                            }
                        }
                    }
                case TipoToken.TK_STRING:
                    {
                        currentToken = lex.NextToken();
                        return new Cadena();
                    }
                default: throw new Exception("Eso no es un tipo.");
            }
        }

        Expresiones Expr()
        {
            try
            {
                return exPrime(andExp());
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones exPrime(Expresiones par)
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                try
                {
                    return exPrime(new Or(par, andExp()));
                }
                catch (Exception ex) { throw ex; }
            }
            else return par;
        }

        Expresiones andExp()
        {
            try
            {
                return andexPrime(relexp());
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones andexPrime(Expresiones par)
        {
            if (currentToken.Tipo == TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                try
                {
                    return andexPrime(new And(par, relexp()));
                }
                catch (Exception ex) { throw ex; }
            }
            else return par;
        }

        Expresiones relexp()
        {
            try
            {
                return relexprime(addexp());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Expresiones relexprime(Expresiones par)
        {
            try
            {
                switch (currentToken.Tipo)
                {
                    case TipoToken.TK_IGUALDAD:
                        currentToken = lex.NextToken();
                        return new Equal(par, addexp());
                    case TipoToken.TK_DISTINTO:
                        currentToken = lex.NextToken();
                        return new Distinto(par, addexp());
                    case TipoToken.TK_MAYORIGUAL:
                        currentToken = lex.NextToken();
                        return new MayorIgual(par, addexp());
                    case TipoToken.TK_MAYORQUE:
                        currentToken = lex.NextToken();
                        return new MayorQue(par, addexp());
                    case TipoToken.TK_MENORIGUAL:
                        currentToken = lex.NextToken();
                        return new MenorIgual(par, addexp());
                    case TipoToken.TK_MENORQUE:
                        currentToken = lex.NextToken();
                        return new MenorQue(par, addexp());
                    default:
                        return par;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones addexp()
        {
            try
            {
                return addexprime(multexp());
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones addexprime(Expresiones par)
        {
            try
            {
                switch (currentToken.Tipo)
                {
                    case TipoToken.TK_SUMA:
                        currentToken = lex.NextToken();
                        return new Suma(par, addexprime(multexp()));
                    case TipoToken.TK_RESTA:
                        currentToken = lex.NextToken();
                        return new Resta(par, addexprime(multexp()));
                    default:
                        return par;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones multexp()
        {
            try
            {
                return multexprime(parexp());
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones multexprime(Expresiones par)
        {
            try{
                switch (currentToken.Tipo)
                {
                    case TipoToken.TK_MULT:
                        currentToken = lex.NextToken();
                        return new Multiplicacion(par, multexprime(parexp()));
                    case TipoToken.TK_DIVISION:
                        currentToken = lex.NextToken();
                        return new Division(par, multexprime(parexp()));
                    case TipoToken.TK_MOD:
                        currentToken = lex.NextToken();
                        return new Mod(par, multexprime(parexp()));
                    case TipoToken.TK_DIV:
                        currentToken = lex.NextToken();
                        return new Division(par, multexprime(parexp()));
                    default:
                        return par;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        Expresiones parexp()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Expresiones tmp;
                try
                {
                    tmp = Expr();
                }
                catch (Exception ex) { throw ex; }
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return tmp;
                }
                else throw new Exception("Se esperaba )");
            }
            else if (currentToken.Tipo == TipoToken.TK_NOT)
            {
                currentToken = lex.NextToken();
                try
                {
                    return new Not(Expr());
                }
                catch (Exception ex) { throw ex; }
            }
            else if (currentToken.Tipo == TipoToken.TK_ID)
            {
                string tmp = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    ExprFuncion ret = new ExprFuncion();
                    ret.ID = new Variable(tmp, null);
                    ret.VarList = new ListaExpre();
                    try
                    {
                        ret.VarList.Ex = ExprList();
                    }
                    catch (Exception ex) { throw ex; }
                    if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba ).");
                    else
                    {
                        currentToken = lex.NextToken();
                        return ret;
                    }
                }
                else
                {
                    try
                    {
                        return new Variable(tmp, AccessList(tmp));
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            else return LIT();
        }

        Expresiones LIT()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT_LIT:
                    LiteralEntero asd = new LiteralEntero(int.Parse(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return asd;
                case TipoToken.TK_FLOAT_LIT:
                    LiteralFlotante flo = new LiteralFlotante(float.Parse(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return flo;
                case TipoToken.TK_CHAR_LIT:
                    string car = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    return new LitChar(car);
                case TipoToken.TK_STRING_LIT:
                    string cad = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    return new LitString(cad);
                case TipoToken.TK_TRUE:
                    currentToken = lex.NextToken();
                    return new LitBool(true);
                case TipoToken.TK_FALSE:
                    currentToken = lex.NextToken();
                    return new LitBool(false);
                default:
                    throw new Exception("Se esperaba una literal");
            }
        }
    }
}