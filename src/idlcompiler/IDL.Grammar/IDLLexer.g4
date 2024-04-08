lexer grammar IDLLexer;

channels { COMMENTS_CHANNEL }

INTEGER_LITERAL: ('0' | '1' .. '9' '0' .. '9'*) INTEGER_TYPE_SUFFIX?;

OCTAL_LITERAL: '0' ('0' .. '7')+ INTEGER_TYPE_SUFFIX?;

HEX_LITERAL: '0' ('x' | 'X') HEX_DIGIT+ INTEGER_TYPE_SUFFIX?;

fragment HEX_DIGIT: ('0' .. '9' | 'a' .. 'f' | 'A' .. 'F');

fragment INTEGER_TYPE_SUFFIX: ('l' | 'L');

FLOATING_PT_LITERAL: ('0' .. '9')+ '.' ('0' .. '9')* EXPONENT? FLOAT_TYPE_SUFFIX?
	| '.' ('0' .. '9')+ EXPONENT? FLOAT_TYPE_SUFFIX?
	| ('0' .. '9')+ EXPONENT FLOAT_TYPE_SUFFIX?
	| ('0' .. '9')+ EXPONENT? FLOAT_TYPE_SUFFIX;

FIXED_PT_LITERAL: FLOATING_PT_LITERAL;

fragment EXPONENT: ('e' | 'E') (PLUS | MINUS)? ('0' .. '9')+;

fragment FLOAT_TYPE_SUFFIX: ('f' | 'F' | 'd' | 'D');

WIDE_CHARACTER_LITERAL: 'L' CHARACTER_LITERAL;

CHARACTER_LITERAL: '\'' (ESCAPE_SEQUENCE | ~ ('\'' | '\\')) '\'';

WIDE_STRING_LITERAL: 'L' STRING_LITERAL;

STRING_LITERAL: '"' (ESCAPE_SEQUENCE | ~ ('\\' | '"'))* '"';

BOOLEAN_LITERAL: 'TRUE' | 'FALSE';

fragment ESCAPE_SEQUENCE:
	'\\' ('b' | 't' | 'n' | 'f' | 'r' | '"' | '\'' | '\\')
	| UNICODE_ESCAPE
	| OCTAL_ESCAPE;

fragment OCTAL_ESCAPE:
	'\\' ('0' .. '3') ('0' .. '7') ('0' .. '7')
	| '\\' ('0' .. '7') ('0' .. '7')
	| '\\' ('0' .. '7');

fragment UNICODE_ESCAPE:
	'\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT;

fragment LETTER:
	'\u0024'
	| '\u0041' .. '\u005a'
	| '\u005f'
	| '\u0061' .. '\u007a'
	| '\u00c0' .. '\u00d6'
	| '\u00d8' .. '\u00f6'
	| '\u00f8' .. '\u00ff'
	| '\u0100' .. '\u1fff'
	| '\u3040' .. '\u318f'
	| '\u3300' .. '\u337f'
	| '\u3400' .. '\u3d2d'
	| '\u4e00' .. '\u9fff'
	| '\uf900' .. '\ufaff';

fragment ID_DIGIT:
	'\u0030' .. '\u0039'
	| '\u0660' .. '\u0669'
	| '\u06f0' .. '\u06f9'
	| '\u0966' .. '\u096f'
	| '\u09e6' .. '\u09ef'
	| '\u0a66' .. '\u0a6f'
	| '\u0ae6' .. '\u0aef'
	| '\u0b66' .. '\u0b6f'
	| '\u0be7' .. '\u0bef'
	| '\u0c66' .. '\u0c6f'
	| '\u0ce6' .. '\u0cef'
	| '\u0d66' .. '\u0d6f'
	| '\u0e50' .. '\u0e59'
	| '\u0ed0' .. '\u0ed9'
	| '\u1040' .. '\u1049';

fragment A: [aA];
fragment B: [bB];
fragment C: [cC];
fragment D: [dD];
fragment E: [eE];
fragment F: [fF];
fragment G: [gG];
fragment H: [hH];
fragment I: [iI];
fragment J: [jJ];
fragment K: [kK];
fragment L: [lL];
fragment M: [mM];
fragment N: [nN];
fragment O: [oO];
fragment P: [pP];
fragment Q: [qQ];
fragment R: [rR];
fragment S: [sS];
fragment T: [tT];
fragment U: [uU];
fragment V: [vV];
fragment W: [wW];
fragment X: [xX];
fragment Y: [yY];
fragment Z: [zZ];

SEMICOLON: ';';

COLON: ':';

COMMA: ',';

LEFT_BRACE: '{';

RIGHT_BRACE: '}';

LEFT_BRACKET: '(';

RIGHT_BRACKET: ')';

LEFT_SQUARE_BRACKET: '[';

RIGHT_SQUARE_BRACKET: ']';

TILDE: '~';

SLASH: '/';

LEFT_ANG_BRACKET: '<';

RIGHT_ANG_BRACKET: '>';

STAR: '*';

PLUS: '+';

MINUS: '-';

CARET: '^';

AMPERSAND: '&';

PIPE: '|';

EQUAL: '=';

PERCENT: '%';

DOUBLE_COLON: '::';

RIGHT_SHIFT: '>>';

LEFT_SHIFT: '<<';

AT: '@';

KW_SETRAISES: 'setraises';

KW_OUT: 'out';

KW_EMITS: 'emits';

KW_STRING: 'string';

KW_SWITCH: 'switch';

KW_PUBLISHES: 'publishes';

KW_TYPEDEF: 'typedef';

KW_USES: 'uses';

KW_PRIMARYKEY: 'primarykey';

KW_CUSTOM: 'custom';

KW_OCTET: 'octet';

KW_SEQUENCE: 'sequence';

KW_IMPORT: 'import';

KW_STRUCT: 'struct';

KW_NATIVE: 'native';

KW_READONLY: 'readonly';

KW_FINDER: 'finder';

KW_RAISES: 'raises';

KW_VOID: 'void';

KW_PRIVATE: 'private';

KW_EVENTTYPE: 'eventtype';

KW_WCHAR: 'wchar';

KW_IN: 'in';

KW_DEFAULT: 'default';

KW_PUBLIC: 'public';

KW_SHORT: 'short';

KW_LONG: 'long';

KW_ENUM: 'enum';

KW_WSTRING: 'wstring';

KW_CONTEXT: 'context';

KW_HOME: 'home';

KW_FACTORY: 'factory';

KW_EXCEPTION: 'exception';

KW_GETRAISES: 'getraises';

KW_CONST: 'const';

KW_VALUEBASE: 'ValueBase';

KW_VALUETYPE: 'valuetype';

KW_SUPPORTS: 'supports';

KW_MODULE: 'module';

KW_OBJECT: 'Object';

KW_TRUNCATABLE: 'truncatable';

KW_UNSIGNED: 'unsigned';

KW_FIXED: 'fixed';

KW_UNION: 'union';

KW_ONEWAY: 'oneway';

KW_ANY: 'any';

KW_CHAR: 'char';

KW_CASE: 'case';

KW_FLOAT: 'float';

KW_BOOLEAN: 'boolean';

KW_MULTIPLE: 'multiple';

KW_ABSTRACT: 'abstract';

KW_INOUT: 'inout';

KW_PROVIDES: 'provides';

KW_CONSUMES: 'consumes';

KW_DOUBLE: 'double';

KW_TYPEPREFIX: 'typeprefix';

KW_TYPEID: 'typeid';

KW_ATTRIBUTE: 'attribute';

KW_LOCAL: 'local';

KW_MANAGES: 'manages';

KW_INTERFACE: 'interface';

KW_COMPONENT: 'component';

KW_MAP: 'map';

KW_BITFIELD: 'bitfield';

KW_BITSET: 'bitset';

KW_BITMASK: 'bitmask';

KW_INT8: 'int8';

KW_UINT8: 'uint8';

KW_INT16: 'int16';

KW_UINT16: 'uint16';

KW_INT32: 'int32';

KW_UINT32: 'uint32';

KW_INT64: 'int64';

KW_UINT64: 'uint64';

KW_AT_ANNOTATION: '@annotation';

ID: LETTER (LETTER | ID_DIGIT)*;

WS: (' ' | '\r' | '\t' | '\u000C' | '\n') -> channel (HIDDEN);

COMMENT: '/*' .*? '*/' -> channel (HIDDEN);

LINE_COMMENT:
	'//' ~ ('\n' | '\r')* '\r'? '\n' -> channel (HIDDEN);

SHARP: '#' -> mode(DIRECTIVE_MODE);

mode DIRECTIVE_MODE;

INCLUDE: 'include' [ \t]+ 			-> mode(DIRECTIVE_TEXT);
LINE: 'line'						-> mode(DEFAULT_MODE);
PRAGMA:  'pragma' 					-> mode(DIRECTIVE_TEXT);
PRAGMA_VERSION: 'pragma version'	-> mode(PRAGMA_VERSION_MODE);
PRAGMA_PREFIX: 'pragma prefix'		-> mode(DEFAULT_MODE);
PRAGMA_ID: 'pragma ID'				-> mode(DEFAULT_MODE);
DEFINE:  'define' [ \t]+ 			-> mode(DIRECTIVE_DEFINE);
DEFINED: 'defined';
IF:      'if';
ELIF:    'elif';
ELSE:    'else';
UNDEF:   'undef';
IFDEF:   'ifdef';
IFNDEF:  'ifndef';
ENDIF:   'endif';
TRUE:     T R U E;
FALSE:    F A L S E;
ERROR:   'error' 					-> mode(DIRECTIVE_TEXT);

OP_BANG:             '!' ;
OP_LPAREN:           '(' ;
OP_RPAREN:           ')' ;
OP_EQUAL:            '==';
OP_NOTEQUAL:         '!=';
OP_AND:              '&&';
OP_OR:               '||';
OP_LT:               '<' ;
OP_GT:               '>' ;
OP_LE:               '<=';
OP_GE:               '>=';

DIRECTIVE_WHITESPACES:      [ \t]+                           -> channel(HIDDEN);
DIRECTIVE_STRING:           '"' (~('\\' | '"') | '\\' .)* '"';
CONDITIONAL_SYMBOL:         LETTER (LETTER | [0-9])*;
DECIMAL_LITERAL:            [0-9]+;
FLOAT:                      ([0-9]+ '.' [0-9]* | '.' [0-9]+);
NEW_LINE:                   '\r'? '\n'                       -> mode(DEFAULT_MODE);
DIRECTIVE_COMMENT:          '/*' .*? '*/'                    -> channel(COMMENTS_CHANNEL);
DIRECTIVE_LINE_COMMENT:     '//' ~[\r\n]*                    -> channel(COMMENTS_CHANNEL);
DIRECTIVE_NEW_LINE:         '\\' '\r'? '\n'                  -> channel(HIDDEN);

mode DIRECTIVE_DEFINE;

DIRECTIVE_DEFINE_CONDITIONAL_SYMBOL: LETTER (LETTER | [0-9])* ('(' (LETTER | [0-9,. \t])* ')')? -> type(CONDITIONAL_SYMBOL), mode(DIRECTIVE_TEXT);

mode DIRECTIVE_TEXT;

DIRECITVE_TEXT_NEW_LINE:    '\\' '\r'? '\n'  -> channel(HIDDEN);
BACK_SLASH_ESCAPE:          '\\' .           -> type(TEXT);
TEXT_NEW_LINE:              '\r'? '\n'       -> type(NEW_LINE), mode(DEFAULT_MODE);
TEXT_COMMENT:               '/*' .*? '*/'    -> channel(COMMENTS_CHANNEL), type(DIRECTIVE_COMMENT);
TEXT_LINE_COMMENT:          '//' ~[\r\n]*    -> channel(COMMENTS_CHANNEL), type(DIRECTIVE_LINE_COMMENT);
DIRECTIVE_SLASH:            '/'              -> type(TEXT);
DIRECTIVE_SEMICOLON:        ';'              -> type(TEXT);
TEXT:                       ~[\r\n\\/]+;

mode PRAGMA_VERSION_MODE;

SCOPED_NAME: (DOUBLE_COLON)? ID (DOUBLE_COLON ID)*;
VERSION_NUM: ('0' | '1' .. '9' '0' .. '9'*) '.' ('0' | '1' .. '9' '0' .. '9'*) -> mode(DEFAULT_MODE);