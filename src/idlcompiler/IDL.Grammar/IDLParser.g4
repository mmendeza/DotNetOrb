parser grammar IDLParser;

options { tokenVocab=IDLLexer; }

specification
   : definition* EOF
   ;

definition
   : type_decl SEMICOLON
   | const_decl SEMICOLON
   | except_decl SEMICOLON
   | interface_or_forward_decl SEMICOLON
   | module SEMICOLON
   | value SEMICOLON
   | type_id_decl SEMICOLON?
   | type_prefix_decl SEMICOLON?
   | event SEMICOLON
   | component SEMICOLON
   | home_decl SEMICOLON
   | annotation_decl SEMICOLON
   | include
   | SHARP directive NEW_LINE
   | SHARP pragma
   | SHARP line   
   ;

module
   : annapps KW_MODULE ID LEFT_BRACE definition + RIGHT_BRACE
   ;

interface_or_forward_decl
   : annapps (interface_decl | forward_decl)
   ;

interface_decl
   : interface_header LEFT_BRACE interface_body RIGHT_BRACE
   ;

forward_decl
   : (KW_ABSTRACT | KW_LOCAL)? KW_INTERFACE ID
   ;

interface_header
   : (KW_ABSTRACT | KW_LOCAL)? KW_INTERFACE ID (interface_inheritance_spec)?
   ;

interface_body
   : interface_export*
   ;

interface_export
   : type_decl SEMICOLON
   | const_decl SEMICOLON
   | except_decl SEMICOLON
   | attr_decl SEMICOLON
   | op_decl SEMICOLON
   | type_id_decl SEMICOLON?
   | type_prefix_decl SEMICOLON?     
   ;

interface_inheritance_spec
   : COLON interface_name (COMMA interface_name)*
   ;

interface_name
   : scoped_name
   ;

scoped_name
   : (DOUBLE_COLON)? ID (DOUBLE_COLON ID)*
   ;

value
   : annapps
     (value_decl | value_abs_decl | value_box_decl | value_forward_decl)
   ;
 
value_forward_decl
   : (KW_ABSTRACT)? KW_VALUETYPE ID
   ;

value_box_decl
   : KW_VALUETYPE ID type_spec
   ;

value_abs_decl
   : KW_ABSTRACT KW_VALUETYPE ID value_inheritance_spec LEFT_BRACE interface_export* RIGHT_BRACE
   ;

value_decl
   : value_header LEFT_BRACE value_element* RIGHT_BRACE
   ;

value_header
   : (KW_CUSTOM)? KW_VALUETYPE ID value_inheritance_spec
   ;

value_inheritance_spec
   : (COLON (KW_TRUNCATABLE)? value_name (COMMA value_name)*)? (KW_SUPPORTS interface_name (COMMA interface_name)*)?
   ;

value_name
   : scoped_name
   ;

value_element
   : (interface_export | state_member | init_decl)
   ;

state_member
   : annapps ( KW_PUBLIC | KW_PRIVATE ) type_spec declarators SEMICOLON
   ;

init_decl
   : annapps KW_FACTORY ID LEFT_BRACKET (init_param_decls)? RIGHT_BRACKET (raises_expr)? SEMICOLON
   ;

init_param_decls
   : init_param_decl (COMMA init_param_decl)*
   ;

init_param_decl
   : annapps init_param_attribute param_type_spec simple_declarator
   ;

init_param_attribute
   : KW_IN
   ;

const_decl
   : annapps KW_CONST const_type ID EQUAL const_exp
   ;

const_type
   : integer_type
   | char_type
   | wide_char_type
   | boolean_type
   | floating_pt_type
   | string_type
   | wide_string_type
   | fixed_pt_const_type
   | scoped_name
   | octet_type     
   ;

const_exp
   : or_expr
   ;

or_expr
   : xor_expr (PIPE xor_expr)*
   ;

xor_expr
   : and_expr (CARET and_expr)*
   ;

and_expr
   : shift_expr (AMPERSAND shift_expr)*
   ;

shift_expr
   : add_expr ((RIGHT_SHIFT | LEFT_SHIFT) add_expr)*
   ;

add_expr
   : mult_expr ((PLUS | MINUS) mult_expr)*
   ;

mult_expr
   : unary_expr ((STAR | SLASH | PERCENT) unary_expr)*
   ;

unary_expr
   : unary_operator primary_expr
   | primary_expr
   ;

unary_operator
   : (MINUS | PLUS | TILDE)
   ;

primary_expr
   : scoped_name
   | literal
   | LEFT_BRACKET const_exp RIGHT_BRACKET
   ;

literal
   : (HEX_LITERAL | INTEGER_LITERAL | OCTAL_LITERAL | STRING_LITERAL | WIDE_STRING_LITERAL | CHARACTER_LITERAL | WIDE_CHARACTER_LITERAL | FIXED_PT_LITERAL | FLOATING_PT_LITERAL | BOOLEAN_LITERAL)
   ;

positive_int_const
   : const_exp
   ;

type_decl
   : type_def
   | struct_type
   | union_type
   | enum_type
   | bitset_type
   | bitmask_type
   | native_type
   | constr_forward_decl
   ;

type_def
   : annapps KW_TYPEDEF type_declarator
   ;

native_type
   : annapps KW_NATIVE simple_declarators
   ;

type_declarator
   : type_spec declarators
   ;

type_spec
   : simple_type_spec
   | constr_type_spec
   ;

simple_type_spec
   : base_type_spec
   | template_type_spec
   | scoped_name
   ;

bitfield_type_spec
   : integer_type
   | boolean_type
   | octet_type
   ;

base_type_spec
   : floating_pt_type
   | integer_type
   | char_type
   | wide_char_type
   | boolean_type
   | octet_type
   | any_type
   | object_type
   | value_base_type
   ;

template_type_spec
   : sequence_type
   | map_type
   | string_type
   | wide_string_type
   | fixed_pt_type
   ;

constr_type_spec
   : struct_type
   | union_type
   | enum_type
   | bitset_type
   | bitmask_type
   ;

simple_declarators
   : ID (COMMA ID)*
   ;

declarators
   : declarator (COMMA declarator)*
   ;

declarator
   : simple_declarator | complex_declarator
   ;

simple_declarator
   : ID
   ;

complex_declarator
   : array_declarator
   ;

floating_pt_type
   : (KW_FLOAT | KW_DOUBLE | KW_LONG KW_DOUBLE)
   ;

integer_type
   : signed_int
   | unsigned_int
   ;

signed_int
   : signed_short_int
   | signed_long_int
   | signed_longlong_int
   | signed_tiny_int
   ;

signed_tiny_int
   : KW_INT8
   ;

signed_short_int
   : KW_SHORT
   | KW_INT16
   ;

signed_long_int
   : KW_LONG
   | KW_INT32
   ;

signed_longlong_int
   : KW_LONG KW_LONG
   | KW_INT64
   ;

unsigned_int
   : unsigned_short_int
   | unsigned_long_int
   | unsigned_longlong_int
   | unsigned_tiny_int
   ;

unsigned_tiny_int
   : KW_UINT8
   ;

unsigned_short_int
   : KW_UNSIGNED KW_SHORT
   | KW_UINT16
   ;

unsigned_long_int
   : KW_UNSIGNED KW_LONG
   | KW_UINT32
   ;

unsigned_longlong_int
   : KW_UNSIGNED KW_LONG KW_LONG
   | KW_UINT64
   ;

char_type
   : KW_CHAR
   ;

wide_char_type
   : KW_WCHAR
   ;

boolean_type
   : KW_BOOLEAN
   ;

octet_type
   : KW_OCTET
   ;

any_type
   : KW_ANY
   ;

object_type
   : KW_OBJECT
   ;

annotation_decl
   : annotation_def
   | annotation_forward_dcl
   ;

annotation_def
   : annotation_header LEFT_BRACE annotation_body RIGHT_BRACE
   ;

annotation_header
   : KW_AT_ANNOTATION ID (annotation_inheritance_spec)?
   ;

annotation_inheritance_spec
   : COLON scoped_name
   ;

annotation_body
   : annotation_export*
   ;

annotation_export
   : annotation_member
   | enum_type SEMICOLON
   | const_decl SEMICOLON
   | type_def SEMICOLON
   ;

annotation_member
   : annotation_member_type simple_declarator (KW_DEFAULT const_exp)? SEMICOLON
   ;

annotation_member_type
   : const_type
   | any_type
   ;

annotation_forward_dcl
   : KW_AT_ANNOTATION ID
   ;

bitset_type
   : annapps KW_BITSET ID (COLON scoped_name)? LEFT_BRACE bitfield + RIGHT_BRACE
   ;

bitfield
   : bitfield_spec (simple_declarators)? SEMICOLON
   ;

bitfield_spec
   : annapps KW_BITFIELD LEFT_ANG_BRACKET positive_int_const (COMMA bitfield_type_spec)? RIGHT_ANG_BRACKET
   ;

bitmask_type
   : annapps KW_BITMASK ID LEFT_BRACE bit_value (COMMA bit_value)* RIGHT_BRACE
   ;

bit_value
   : annapps ID
   ;

struct_type
   : annapps KW_STRUCT ID (COLON scoped_name)? LEFT_BRACE member_list RIGHT_BRACE
   ;

member_list
   : member *
   ;

member
   : annapps type_spec declarators SEMICOLON
   ;

union_type
   : annapps KW_UNION ID KW_SWITCH LEFT_BRACKET switch_type_spec RIGHT_BRACKET LEFT_BRACE switch_body RIGHT_BRACE
   ;

switch_type_spec
   : integer_type
   | char_type
   | wide_char_type
   | octet_type
   | boolean_type
   | enum_type
   | scoped_name
   ;

switch_body
   : case_stmt +
   ;

case_stmt
   : case_label + element_spec SEMICOLON
   ;

case_label
   : KW_CASE const_exp COLON
   | KW_DEFAULT COLON
   ;

element_spec
   : annapps type_spec declarator
   ;

enum_type
   : annapps KW_ENUM ID LEFT_BRACE enumerator (COMMA enumerator)* RIGHT_BRACE
   ;

enumerator
   : annapps ID
   ;

sequence_type
   : KW_SEQUENCE LEFT_ANG_BRACKET simple_type_spec (COMMA positive_int_const)? RIGHT_ANG_BRACKET
   ;

map_type
   : KW_MAP LEFT_ANG_BRACKET simple_type_spec COMMA simple_type_spec (COMMA positive_int_const)?  RIGHT_ANG_BRACKET
   ;

string_type
   : KW_STRING (LEFT_ANG_BRACKET positive_int_const RIGHT_ANG_BRACKET)?
   ;

wide_string_type
   : KW_WSTRING (LEFT_ANG_BRACKET positive_int_const RIGHT_ANG_BRACKET)?
   ;

array_declarator
   : ID fixed_array_size +
   ;

fixed_array_size
   : LEFT_SQUARE_BRACKET positive_int_const RIGHT_SQUARE_BRACKET
   ;

attr_decl
   : annapps (readonly_attr_spec | attr_spec)
   ;

except_decl
   : annapps KW_EXCEPTION ID LEFT_BRACE member* RIGHT_BRACE
   ;

op_decl
   : annapps (op_attribute)? op_type_spec ID parameter_decls (raises_expr)? (context_expr)?
   ;

op_attribute
   : KW_ONEWAY
   ;

op_type_spec
   : param_type_spec | KW_VOID
   ;

parameter_decls
   : LEFT_BRACKET ( param_decl (COMMA param_decl)* )? RIGHT_BRACKET
   ;

param_decl
   : annapps param_attribute param_type_spec simple_declarator
   ;

param_attribute
   : KW_IN
   | KW_OUT
   | KW_INOUT
   ;

raises_expr
   : KW_RAISES LEFT_BRACKET scoped_name (COMMA scoped_name)* RIGHT_BRACKET
   ;

context_expr
   : KW_CONTEXT LEFT_BRACKET STRING_LITERAL (COMMA STRING_LITERAL)* RIGHT_BRACKET
   ;

param_type_spec
   : base_type_spec
   | string_type
   | wide_string_type
   | scoped_name
   ;

fixed_pt_type
   : KW_FIXED LEFT_ANG_BRACKET positive_int_const COMMA positive_int_const RIGHT_ANG_BRACKET
   ;

fixed_pt_const_type
   : KW_FIXED
   ;

value_base_type
   : KW_VALUEBASE
   ;

constr_forward_decl
   : KW_STRUCT ID
   | KW_UNION ID
   ;

import_decl
   : KW_IMPORT imported_scope
   ;

imported_scope
   : scoped_name
   | STRING_LITERAL
   ;

type_id_decl
   : KW_TYPEID scoped_name STRING_LITERAL
   ;

type_prefix_decl
   : KW_TYPEPREFIX scoped_name STRING_LITERAL
   ;

readonly_attr_spec
   : KW_READONLY KW_ATTRIBUTE param_type_spec readonly_attr_declarator
   ;

readonly_attr_declarator
   : simple_declarator
     ( raises_expr
     | (COMMA simple_declarator)*
     )
   ;

attr_spec
   : KW_ATTRIBUTE param_type_spec attr_declarator
   ;

attr_declarator
   : simple_declarator
     ( attr_raises_expr
     | (COMMA simple_declarator)*
     )
   ;

attr_raises_expr
   : get_excep_expr (set_excep_expr)?
   | set_excep_expr
   ;

get_excep_expr
   : KW_GETRAISES exception_list
   ;

set_excep_expr
   : KW_SETRAISES exception_list
   ;

exception_list
   : LEFT_BRACKET scoped_name (COMMA scoped_name)* RIGHT_BRACKET
   ;

component
   : component_decl
   | component_forward_decl
   ;

component_forward_decl
   : annapps KW_COMPONENT ID
   ;

component_decl
   : annapps component_header LEFT_BRACE component_body RIGHT_BRACE
   ;

component_header
   : KW_COMPONENT ID (component_inheritance_spec)? (supported_interface_spec)?
   ;

supported_interface_spec
   : KW_SUPPORTS scoped_name (COMMA scoped_name)*
   ;

component_inheritance_spec
   : COLON scoped_name
   ;

component_body
   : component_export*
   ;

component_export
   : annapps
     ( provides_decl SEMICOLON
     | uses_decl SEMICOLON
     | emits_decl SEMICOLON
     | publishes_decl SEMICOLON
     | consumes_decl SEMICOLON
     | attr_decl SEMICOLON
     )
   ;

provides_decl
   : KW_PROVIDES interface_type ID
   ;

interface_type
   : scoped_name | KW_OBJECT     
   ;

uses_decl
   : KW_USES (KW_MULTIPLE)? interface_type ID
   ;

emits_decl
   : KW_EMITS scoped_name ID
   ;

publishes_decl
   : KW_PUBLISHES scoped_name ID
   ;

consumes_decl
   : KW_CONSUMES scoped_name ID
   ;

home_decl
   : home_header home_body
   ;

home_header
   : KW_HOME ID (home_inheritance_spec)? (supported_interface_spec)? KW_MANAGES scoped_name (primary_key_spec)?
   ;

home_inheritance_spec
   : COLON scoped_name
   ;

primary_key_spec
   : KW_PRIMARYKEY scoped_name
   ;

home_body
   : LEFT_BRACE home_export* RIGHT_BRACE
   ;

home_export
   : interface_export
   | annapps
     ( factory_decl
     | finder_decl
     )
     SEMICOLON
   ;

factory_decl
   : KW_FACTORY ID LEFT_BRACKET (init_param_decls)? RIGHT_BRACKET (raises_expr)?
   ;

finder_decl
   : KW_FINDER ID LEFT_BRACKET (init_param_decls)? RIGHT_BRACKET (raises_expr)?
   ;

event
   : annapps (event_decl | event_abs_decl | event_forward_decl)
   ;

event_forward_decl
   : (KW_ABSTRACT)? KW_EVENTTYPE ID
   ;

event_abs_decl
   : KW_ABSTRACT KW_EVENTTYPE ID value_inheritance_spec LEFT_BRACE interface_export* RIGHT_BRACE
   ;

event_decl
   : event_header LEFT_BRACE value_element* RIGHT_BRACE
   ;

event_header
   : (KW_CUSTOM)? KW_EVENTTYPE ID value_inheritance_spec
   ;

annapps
   : ( annotation_appl )*
   ;

annotation_appl
   : AT scoped_name ( LEFT_BRACKET annotation_appl_params RIGHT_BRACKET )?
   ;

annotation_appl_params
   : const_exp
   | annotation_appl_param ( COMMA annotation_appl_param )*
   ;

annotation_appl_param
   : ID EQUAL const_exp
   ;

pragma
    : PRAGMA_PREFIX STRING_LITERAL                          #pragma_prefix
    | PRAGMA_ID scoped_name STRING_LITERAL                  #pragma_id
    | PRAGMA_VERSION SCOPED_NAME VERSION_NUM                #pragma_version
    ;

line
   : LINE INTEGER_LITERAL STRING_LITERAL
   ;

include
    : SHARP INCLUDE directive_text NEW_LINE                       #preprocessorInclude
    | import_decl SEMICOLON?                                      #preprocessorImport
    ;

directive
    : IF preprocessor_expression                    #preprocessorConditional
    | ELIF preprocessor_expression                  #preprocessorConditional
    | ELSE                                          #preprocessorConditional
    | ENDIF directive_text?                         #preprocessorConditional
    | IFDEF CONDITIONAL_SYMBOL                      #preprocessorDef
    | IFNDEF CONDITIONAL_SYMBOL                     #preprocessorDef
    | UNDEF CONDITIONAL_SYMBOL                      #preprocessorDef
    | DEFINE CONDITIONAL_SYMBOL directive_text?     #preprocessorDefine
    ;

directive_text
    : TEXT+
    ;

preprocessor_expression
    : TRUE                                                                                #preprocessorConstant
    | FALSE                                                                               #preprocessorConstant
    | DECIMAL_LITERAL                                                                     #preprocessorConstant
    | DIRECTIVE_STRING                                                                    #preprocessorConstant
    | CONDITIONAL_SYMBOL (OP_LPAREN preprocessor_expression OP_RPAREN)?                   #preprocessorConditionalSymbol
    | OP_LPAREN preprocessor_expression OP_RPAREN                                         #preprocessorParenthesis
    | OP_BANG preprocessor_expression                                                     #preprocessorNot
    | preprocessor_expression op=(EQUAL | OP_NOTEQUAL) preprocessor_expression            #preprocessorBinary
    | preprocessor_expression op=OP_AND preprocessor_expression                           #preprocessorBinary
    | preprocessor_expression op=OP_OR preprocessor_expression                            #preprocessorBinary
    | preprocessor_expression op=(OP_LT | OP_GT | OP_LE | OP_GE) preprocessor_expression  #preprocessorBinary
    | DEFINED (CONDITIONAL_SYMBOL | OP_LPAREN CONDITIONAL_SYMBOL OP_RPAREN)               #preprocessorDefined
    ;