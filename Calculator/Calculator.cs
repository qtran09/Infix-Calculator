using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    class Calculator
    {
        public Calculator() { }

        #region Constants and Field Variables
        private const string SYNTAX_ERROR = "Syntax Error";
        private const string INVALID_INPUT = "Invalid Input";
        private const string DIVIDE_BY_ZERO_ERROR = "Divide by zero Error";
        #endregion

        #region Public Methods
        /// <summary>
        /// Calaculates the provided infix expression. 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Computed value from expression. May return a "Syntax Error" or "Invalid Input" error if input is invalid. </returns>
        public string Calculate(string expression)
        {
            Stack<Double> values = new Stack<Double>();
            Stack<Char> operands = new Stack<Char>();
            bool isPreviousValValidExpr = false;
            bool applyNegation = false;
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];
                if (ch == ' ') continue;

                //Current character is indicative of the beginning of a decimal number
                if (Char.IsDigit(ch) || ch == '.')
                {
                    string extractedNumber = ExtractNumber(expression.Substring(i));
                    if (!Double.TryParse(extractedNumber, out double res)) return SYNTAX_ERROR;
                    if (applyNegation)
                    {
                        res *= -1;
                        applyNegation = false;
                    }
                    values.Push(res);
                    i += (extractedNumber.Length - 1);
                    isPreviousValValidExpr = true;
                }

                //Handle parentheses. When a closed parentheses is found, perform operations until an open parentheses is released from the stack
                else if (ch == '(')
                {
                    operands.Push(ch);
                    isPreviousValValidExpr = false;
                }
                else if (ch == ')')
                {
                    bool closed = false;
                    while (operands.Count > 0)
                    {
                        char operand = operands.Pop();
                        if (operand == '(')
                        {
                            closed = true;
                            break;
                        }
                        if (values.Count < 2) return SYNTAX_ERROR;
                        string computedVal = Operate(values.Pop(), values.Pop(), operand);
                        if (!Double.TryParse(computedVal, out double result)) return computedVal;
                        values.Push(result);
                    }
                    if (!closed) return SYNTAX_ERROR;
                    isPreviousValValidExpr = true;
                }

                //Perform all operations until order of operations doesn't allow us to anymore
                else if (IsOperand(ch))
                {
                    if (ch == '-' && !isPreviousValValidExpr)
                    {
                        if(applyNegation)
                        {
                            return SYNTAX_ERROR;
                        }
                        applyNegation = true;
                        continue;
                    }
                    if (values.Count < 1) return SYNTAX_ERROR;
                    while (operands.Count > 0 && HasPriority(ch, operands.Peek()))
                    {
                        if (values.Count < 2) return SYNTAX_ERROR;
                        string res = Operate(values.Pop(), values.Pop(), operands.Pop());
                        if (!Double.TryParse(res, out double result)) return res;
                        values.Push(result);
                    }
                    operands.Push(ch);
                    isPreviousValValidExpr = false;
                }
                else
                {
                    return INVALID_INPUT;
                }
            }

            //Finish any remaining operations not yet completed
            while (operands.Count > 0)
            {
                if (values.Count < 2) return SYNTAX_ERROR;
                string res = Operate(values.Pop(), values.Pop(), operands.Pop());
                if (!Double.TryParse(res, out double result)) return res;
                values.Push(result);
            }
            if (values.Count > 1) return SYNTAX_ERROR;
            return (values.Count == 1) ? values.Pop().ToString() : "0";
        }
        #endregion

        #region Private helpers
        /// <summary>
        /// Extracts a decimal number from expression. Stops when a non-numeric character is encountered or if there is a syntax error in the number
        /// </summary>
        /// <param name="expression">Substring of provided expression</param>
        /// <returns>Extracted number from given expression. Returns "Syntax Error" if number is invalid</returns>
        private string ExtractNumber(string expression)
        {
            StringBuilder sb = new StringBuilder();
            bool decimalPlaced = false;
            int idx = 0;
            while (idx < expression.Length)
            {
                char ch = expression[idx++];

                if (ch == '.')
                {
                    if (decimalPlaced) return SYNTAX_ERROR;
                    decimalPlaced = true;
                }
                else if (!Char.IsDigit(ch)) return sb.ToString();
                sb.Append(ch);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether operand2 has higher or equal priority over operand1 in regards to order of operations. 
        /// </summary>
        /// <param name="operand1"></param>
        /// <param name="operand2"></param>
        /// <returns>True if operand2 has greater or equal priority than operand1 according to order of operations. False otherwise</returns>
        private bool HasPriority(char operand1, char operand2)
        {
            if (operand2 == '(' || operand2 == ')') return false;
            if ((operand1 == '*' || operand1 == '/') && (operand2 == '+' || operand2 == '-')) return false;
            return true;
        }

        /// <summary>
        /// Checks if a character is a valid operand.
        /// </summary>
        /// <param name="character"></param>
        /// <returns>True if operand is allowed(currently allow addition, subtraction, multiplication, and division). False otherwise</returns>
        private bool IsOperand(char character)
        {
            return character == '+' || character == '-' || character == '/' || character == '*';
        }


        /// <summary>
        /// Performs operation on num2 and num1, according to provided operand
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="operand">Current allowed operands are "+,-,/,*"</param>
        /// <returns>Computed value between num2 and num1 according to operand type. Returns type string to handle possible error message</returns>
        private string Operate(double num1, double num2, char operand)
        {
            switch (operand)
            {
                case '+':
                    return (num2 + num1).ToString();
                case '-':
                    return (num2 - num1).ToString();
                case '*':
                    return (num2 * num1).ToString();
                case '/':
                    if (num1 == 0) return DIVIDE_BY_ZERO_ERROR;
                    return (num2 / num1).ToString();
                default:
                    return INVALID_INPUT;
            }
        }
        #endregion
    }
}
