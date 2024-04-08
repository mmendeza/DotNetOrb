parser grammar IDLPreprocessorParser;

options { tokenVocab=IDLPreprocessorLexer; }

idlDocument
    : text* EOF
    ;

text    
    : SHARP include (NEW_LINE | EOF)
    | SHARP directive (NEW_LINE | EOF)
    | SHARP pragma (NEW_LINE | EOF)
    | code
    ;

code
    : CODE+
    ;

include
    : INCLUDE directive_text                            #preprocessorInclude
    ;

pragma
    : PRAGMA PREFIX directive_text                      #preprocessorPragmaPrefix
    | PRAGMA directive_text                             #preprocessorPragma
    ;

directive    
    : IF preprocessor_expression                    #preprocessorConditional
    | ELIF preprocessor_expression                  #preprocessorConditional
    | ELSE                                          #preprocessorConditional
    | ENDIF directive_text?                         #preprocessorConditional
    | IFDEF CONDITIONAL_SYMBOL                      #preprocessorDef
    | IFNDEF CONDITIONAL_SYMBOL                     #preprocessorDef
    | UNDEF CONDITIONAL_SYMBOL                      #preprocessorDef
    | ERROR directive_text                          #preprocessorError
    | DEFINE CONDITIONAL_SYMBOL directive_text?     #preprocessorDefine
    ;

directive_text
    : TEXT+
    ;

preprocessor_expression
    : TRUE                                                                   #preprocessorConstant
    | FALSE                                                                  #preprocessorConstant
    | DECIMAL_LITERAL                                                        #preprocessorConstant
    | DIRECTIVE_STRING                                                       #preprocessorConstant
    | CONDITIONAL_SYMBOL (LPAREN preprocessor_expression RPAREN)?            #preprocessorConditionalSymbol
    | LPAREN preprocessor_expression RPAREN                                  #preprocessorParenthesis
    | BANG preprocessor_expression                                           #preprocessorNot
    | preprocessor_expression op=(EQUAL | NOTEQUAL) preprocessor_expression  #preprocessorBinary
    | preprocessor_expression op=AND preprocessor_expression                 #preprocessorBinary
    | preprocessor_expression op=OR preprocessor_expression                  #preprocessorBinary
    | preprocessor_expression op=(LT | GT | LE | GE) preprocessor_expression #preprocessorBinary
    | DEFINED (CONDITIONAL_SYMBOL | LPAREN CONDITIONAL_SYMBOL RPAREN)         #preprocessorDefined
    ;