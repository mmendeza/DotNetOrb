// Generated from e:\DotNetOrb\IDL.Grammar\IDLParser.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class IDLParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		INTEGER_LITERAL=1, OCTAL_LITERAL=2, HEX_LITERAL=3, FLOATING_PT_LITERAL=4, 
		FIXED_PT_LITERAL=5, WIDE_CHARACTER_LITERAL=6, CHARACTER_LITERAL=7, WIDE_STRING_LITERAL=8, 
		STRING_LITERAL=9, BOOLEAN_LITERAL=10, SEMICOLON=11, COLON=12, COMMA=13, 
		LEFT_BRACE=14, RIGHT_BRACE=15, LEFT_BRACKET=16, RIGHT_BRACKET=17, LEFT_SQUARE_BRACKET=18, 
		RIGHT_SQUARE_BRACKET=19, TILDE=20, SLASH=21, LEFT_ANG_BRACKET=22, RIGHT_ANG_BRACKET=23, 
		STAR=24, PLUS=25, MINUS=26, CARET=27, AMPERSAND=28, PIPE=29, EQUAL=30, 
		PERCENT=31, DOUBLE_COLON=32, RIGHT_SHIFT=33, LEFT_SHIFT=34, AT=35, KW_SETRAISES=36, 
		KW_OUT=37, KW_EMITS=38, KW_STRING=39, KW_SWITCH=40, KW_PUBLISHES=41, KW_TYPEDEF=42, 
		KW_USES=43, KW_PRIMARYKEY=44, KW_CUSTOM=45, KW_OCTET=46, KW_SEQUENCE=47, 
		KW_IMPORT=48, KW_STRUCT=49, KW_NATIVE=50, KW_READONLY=51, KW_FINDER=52, 
		KW_RAISES=53, KW_VOID=54, KW_PRIVATE=55, KW_EVENTTYPE=56, KW_WCHAR=57, 
		KW_IN=58, KW_DEFAULT=59, KW_PUBLIC=60, KW_SHORT=61, KW_LONG=62, KW_ENUM=63, 
		KW_WSTRING=64, KW_CONTEXT=65, KW_HOME=66, KW_FACTORY=67, KW_EXCEPTION=68, 
		KW_GETRAISES=69, KW_CONST=70, KW_VALUEBASE=71, KW_VALUETYPE=72, KW_SUPPORTS=73, 
		KW_MODULE=74, KW_OBJECT=75, KW_TRUNCATABLE=76, KW_UNSIGNED=77, KW_FIXED=78, 
		KW_UNION=79, KW_ONEWAY=80, KW_ANY=81, KW_CHAR=82, KW_CASE=83, KW_FLOAT=84, 
		KW_BOOLEAN=85, KW_MULTIPLE=86, KW_ABSTRACT=87, KW_INOUT=88, KW_PROVIDES=89, 
		KW_CONSUMES=90, KW_DOUBLE=91, KW_TYPEPREFIX=92, KW_TYPEID=93, KW_ATTRIBUTE=94, 
		KW_LOCAL=95, KW_MANAGES=96, KW_INTERFACE=97, KW_COMPONENT=98, KW_MAP=99, 
		KW_BITFIELD=100, KW_BITSET=101, KW_BITMASK=102, KW_INT8=103, KW_UINT8=104, 
		KW_INT16=105, KW_UINT16=106, KW_INT32=107, KW_UINT32=108, KW_INT64=109, 
		KW_UINT64=110, KW_AT_ANNOTATION=111, ID=112, WS=113, COMMENT=114, LINE_COMMENT=115, 
		VERSION_NUM=116, SHARP=117, INCLUDE=118, LINE=119, PRAGMA=120, PRAGMA_VERSION=121, 
		PRAGMA_PREFIX=122, PRAGMA_ID=123, DEFINE=124, DEFINED=125, IF=126, ELIF=127, 
		ELSE=128, UNDEF=129, IFDEF=130, IFNDEF=131, ENDIF=132, TRUE=133, FALSE=134, 
		ERROR=135, OP_BANG=136, OP_LPAREN=137, OP_RPAREN=138, OP_EQUAL=139, OP_NOTEQUAL=140, 
		OP_AND=141, OP_OR=142, OP_LT=143, OP_GT=144, OP_LE=145, OP_GE=146, DIRECTIVE_WHITESPACES=147, 
		DIRECTIVE_STRING=148, CONDITIONAL_SYMBOL=149, DECIMAL_LITERAL=150, FLOAT=151, 
		NEW_LINE=152, DIRECTIVE_COMMENT=153, DIRECTIVE_LINE_COMMENT=154, DIRECTIVE_NEW_LINE=155, 
		DIRECITVE_TEXT_NEW_LINE=156, TEXT=157, TEXT_NEW_LINE=158;
	public static final int
		RULE_specification = 0, RULE_definition = 1, RULE_module = 2, RULE_interface_or_forward_decl = 3, 
		RULE_interface_decl = 4, RULE_forward_decl = 5, RULE_interface_header = 6, 
		RULE_interface_body = 7, RULE_interface_export = 8, RULE_interface_inheritance_spec = 9, 
		RULE_interface_name = 10, RULE_scoped_name = 11, RULE_value = 12, RULE_value_forward_decl = 13, 
		RULE_value_box_decl = 14, RULE_value_abs_decl = 15, RULE_value_decl = 16, 
		RULE_value_header = 17, RULE_value_inheritance_spec = 18, RULE_value_name = 19, 
		RULE_value_element = 20, RULE_state_member = 21, RULE_init_decl = 22, 
		RULE_init_param_decls = 23, RULE_init_param_decl = 24, RULE_init_param_attribute = 25, 
		RULE_const_decl = 26, RULE_const_type = 27, RULE_const_exp = 28, RULE_or_expr = 29, 
		RULE_xor_expr = 30, RULE_and_expr = 31, RULE_shift_expr = 32, RULE_add_expr = 33, 
		RULE_mult_expr = 34, RULE_unary_expr = 35, RULE_unary_operator = 36, RULE_primary_expr = 37, 
		RULE_literal = 38, RULE_positive_int_const = 39, RULE_type_decl = 40, 
		RULE_type_def = 41, RULE_native_type = 42, RULE_type_declarator = 43, 
		RULE_type_spec = 44, RULE_simple_type_spec = 45, RULE_bitfield_type_spec = 46, 
		RULE_base_type_spec = 47, RULE_template_type_spec = 48, RULE_constr_type_spec = 49, 
		RULE_simple_declarators = 50, RULE_declarators = 51, RULE_declarator = 52, 
		RULE_simple_declarator = 53, RULE_complex_declarator = 54, RULE_floating_pt_type = 55, 
		RULE_integer_type = 56, RULE_signed_int = 57, RULE_signed_tiny_int = 58, 
		RULE_signed_short_int = 59, RULE_signed_long_int = 60, RULE_signed_longlong_int = 61, 
		RULE_unsigned_int = 62, RULE_unsigned_tiny_int = 63, RULE_unsigned_short_int = 64, 
		RULE_unsigned_long_int = 65, RULE_unsigned_longlong_int = 66, RULE_char_type = 67, 
		RULE_wide_char_type = 68, RULE_boolean_type = 69, RULE_octet_type = 70, 
		RULE_any_type = 71, RULE_object_type = 72, RULE_annotation_decl = 73, 
		RULE_annotation_def = 74, RULE_annotation_header = 75, RULE_annotation_inheritance_spec = 76, 
		RULE_annotation_body = 77, RULE_annotation_export = 78, RULE_annotation_member = 79, 
		RULE_annotation_member_type = 80, RULE_annotation_forward_dcl = 81, RULE_bitset_type = 82, 
		RULE_bitfield = 83, RULE_bitfield_spec = 84, RULE_bitmask_type = 85, RULE_bit_value = 86, 
		RULE_struct_type = 87, RULE_member_list = 88, RULE_member = 89, RULE_union_type = 90, 
		RULE_switch_type_spec = 91, RULE_switch_body = 92, RULE_case_stmt = 93, 
		RULE_case_label = 94, RULE_element_spec = 95, RULE_enum_type = 96, RULE_enumerator = 97, 
		RULE_sequence_type = 98, RULE_map_type = 99, RULE_string_type = 100, RULE_wide_string_type = 101, 
		RULE_array_declarator = 102, RULE_fixed_array_size = 103, RULE_attr_decl = 104, 
		RULE_except_decl = 105, RULE_op_decl = 106, RULE_op_attribute = 107, RULE_op_type_spec = 108, 
		RULE_parameter_decls = 109, RULE_param_decl = 110, RULE_param_attribute = 111, 
		RULE_raises_expr = 112, RULE_context_expr = 113, RULE_param_type_spec = 114, 
		RULE_fixed_pt_type = 115, RULE_fixed_pt_const_type = 116, RULE_value_base_type = 117, 
		RULE_constr_forward_decl = 118, RULE_import_decl = 119, RULE_imported_scope = 120, 
		RULE_type_id_decl = 121, RULE_type_prefix_decl = 122, RULE_readonly_attr_spec = 123, 
		RULE_readonly_attr_declarator = 124, RULE_attr_spec = 125, RULE_attr_declarator = 126, 
		RULE_attr_raises_expr = 127, RULE_get_excep_expr = 128, RULE_set_excep_expr = 129, 
		RULE_exception_list = 130, RULE_component = 131, RULE_component_forward_decl = 132, 
		RULE_component_decl = 133, RULE_component_header = 134, RULE_supported_interface_spec = 135, 
		RULE_component_inheritance_spec = 136, RULE_component_body = 137, RULE_component_export = 138, 
		RULE_provides_decl = 139, RULE_interface_type = 140, RULE_uses_decl = 141, 
		RULE_emits_decl = 142, RULE_publishes_decl = 143, RULE_consumes_decl = 144, 
		RULE_home_decl = 145, RULE_home_header = 146, RULE_home_inheritance_spec = 147, 
		RULE_primary_key_spec = 148, RULE_home_body = 149, RULE_home_export = 150, 
		RULE_factory_decl = 151, RULE_finder_decl = 152, RULE_event = 153, RULE_event_forward_decl = 154, 
		RULE_event_abs_decl = 155, RULE_event_decl = 156, RULE_event_header = 157, 
		RULE_annapps = 158, RULE_annotation_appl = 159, RULE_annotation_appl_params = 160, 
		RULE_annotation_appl_param = 161, RULE_pragma = 162, RULE_line = 163, 
		RULE_include = 164, RULE_directive = 165, RULE_directive_text = 166, RULE_preprocessor_expression = 167;
	private static String[] makeRuleNames() {
		return new String[] {
			"specification", "definition", "module", "interface_or_forward_decl", 
			"interface_decl", "forward_decl", "interface_header", "interface_body", 
			"interface_export", "interface_inheritance_spec", "interface_name", "scoped_name", 
			"value", "value_forward_decl", "value_box_decl", "value_abs_decl", "value_decl", 
			"value_header", "value_inheritance_spec", "value_name", "value_element", 
			"state_member", "init_decl", "init_param_decls", "init_param_decl", "init_param_attribute", 
			"const_decl", "const_type", "const_exp", "or_expr", "xor_expr", "and_expr", 
			"shift_expr", "add_expr", "mult_expr", "unary_expr", "unary_operator", 
			"primary_expr", "literal", "positive_int_const", "type_decl", "type_def", 
			"native_type", "type_declarator", "type_spec", "simple_type_spec", "bitfield_type_spec", 
			"base_type_spec", "template_type_spec", "constr_type_spec", "simple_declarators", 
			"declarators", "declarator", "simple_declarator", "complex_declarator", 
			"floating_pt_type", "integer_type", "signed_int", "signed_tiny_int", 
			"signed_short_int", "signed_long_int", "signed_longlong_int", "unsigned_int", 
			"unsigned_tiny_int", "unsigned_short_int", "unsigned_long_int", "unsigned_longlong_int", 
			"char_type", "wide_char_type", "boolean_type", "octet_type", "any_type", 
			"object_type", "annotation_decl", "annotation_def", "annotation_header", 
			"annotation_inheritance_spec", "annotation_body", "annotation_export", 
			"annotation_member", "annotation_member_type", "annotation_forward_dcl", 
			"bitset_type", "bitfield", "bitfield_spec", "bitmask_type", "bit_value", 
			"struct_type", "member_list", "member", "union_type", "switch_type_spec", 
			"switch_body", "case_stmt", "case_label", "element_spec", "enum_type", 
			"enumerator", "sequence_type", "map_type", "string_type", "wide_string_type", 
			"array_declarator", "fixed_array_size", "attr_decl", "except_decl", "op_decl", 
			"op_attribute", "op_type_spec", "parameter_decls", "param_decl", "param_attribute", 
			"raises_expr", "context_expr", "param_type_spec", "fixed_pt_type", "fixed_pt_const_type", 
			"value_base_type", "constr_forward_decl", "import_decl", "imported_scope", 
			"type_id_decl", "type_prefix_decl", "readonly_attr_spec", "readonly_attr_declarator", 
			"attr_spec", "attr_declarator", "attr_raises_expr", "get_excep_expr", 
			"set_excep_expr", "exception_list", "component", "component_forward_decl", 
			"component_decl", "component_header", "supported_interface_spec", "component_inheritance_spec", 
			"component_body", "component_export", "provides_decl", "interface_type", 
			"uses_decl", "emits_decl", "publishes_decl", "consumes_decl", "home_decl", 
			"home_header", "home_inheritance_spec", "primary_key_spec", "home_body", 
			"home_export", "factory_decl", "finder_decl", "event", "event_forward_decl", 
			"event_abs_decl", "event_decl", "event_header", "annapps", "annotation_appl", 
			"annotation_appl_params", "annotation_appl_param", "pragma", "line", 
			"include", "directive", "directive_text", "preprocessor_expression"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			"':'", "','", "'{'", "'}'", null, null, "'['", "']'", "'~'", null, null, 
			null, "'*'", "'+'", "'-'", "'^'", "'&'", "'|'", "'='", "'%'", "'::'", 
			"'>>'", "'<<'", "'@'", "'setraises'", "'out'", "'emits'", "'string'", 
			"'switch'", "'publishes'", "'typedef'", "'uses'", "'primarykey'", "'custom'", 
			"'octet'", "'sequence'", "'import'", "'struct'", "'native'", "'readonly'", 
			"'finder'", "'raises'", "'void'", "'private'", "'eventtype'", "'wchar'", 
			"'in'", "'default'", "'public'", "'short'", "'long'", "'enum'", "'wstring'", 
			"'context'", "'home'", "'factory'", "'exception'", "'getraises'", "'const'", 
			"'ValueBase'", "'valuetype'", "'supports'", "'module'", "'Object'", "'truncatable'", 
			"'unsigned'", "'fixed'", "'union'", "'oneway'", "'any'", "'char'", "'case'", 
			"'float'", "'boolean'", "'multiple'", "'abstract'", "'inout'", "'provides'", 
			"'consumes'", "'double'", "'typeprefix'", "'typeid'", "'attribute'", 
			"'local'", "'manages'", "'interface'", "'component'", "'map'", "'bitfield'", 
			"'bitset'", "'bitmask'", "'int8'", "'uint8'", "'int16'", "'uint16'", 
			"'int32'", "'uint32'", "'int64'", "'uint64'", "'@annotation'", null, 
			null, null, null, null, "'#'", null, "'line'", "'pragma'", "'pragma version'", 
			"'pragma prefix'", "'pragma ID'", null, "'defined'", "'if'", "'elif'", 
			"'else'", "'undef'", "'ifdef'", "'ifndef'", "'endif'", null, null, "'error'", 
			"'!'", null, null, "'=='", "'!='", "'&&'", "'||'", null, null, "'<='", 
			"'>='"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "INTEGER_LITERAL", "OCTAL_LITERAL", "HEX_LITERAL", "FLOATING_PT_LITERAL", 
			"FIXED_PT_LITERAL", "WIDE_CHARACTER_LITERAL", "CHARACTER_LITERAL", "WIDE_STRING_LITERAL", 
			"STRING_LITERAL", "BOOLEAN_LITERAL", "SEMICOLON", "COLON", "COMMA", "LEFT_BRACE", 
			"RIGHT_BRACE", "LEFT_BRACKET", "RIGHT_BRACKET", "LEFT_SQUARE_BRACKET", 
			"RIGHT_SQUARE_BRACKET", "TILDE", "SLASH", "LEFT_ANG_BRACKET", "RIGHT_ANG_BRACKET", 
			"STAR", "PLUS", "MINUS", "CARET", "AMPERSAND", "PIPE", "EQUAL", "PERCENT", 
			"DOUBLE_COLON", "RIGHT_SHIFT", "LEFT_SHIFT", "AT", "KW_SETRAISES", "KW_OUT", 
			"KW_EMITS", "KW_STRING", "KW_SWITCH", "KW_PUBLISHES", "KW_TYPEDEF", "KW_USES", 
			"KW_PRIMARYKEY", "KW_CUSTOM", "KW_OCTET", "KW_SEQUENCE", "KW_IMPORT", 
			"KW_STRUCT", "KW_NATIVE", "KW_READONLY", "KW_FINDER", "KW_RAISES", "KW_VOID", 
			"KW_PRIVATE", "KW_EVENTTYPE", "KW_WCHAR", "KW_IN", "KW_DEFAULT", "KW_PUBLIC", 
			"KW_SHORT", "KW_LONG", "KW_ENUM", "KW_WSTRING", "KW_CONTEXT", "KW_HOME", 
			"KW_FACTORY", "KW_EXCEPTION", "KW_GETRAISES", "KW_CONST", "KW_VALUEBASE", 
			"KW_VALUETYPE", "KW_SUPPORTS", "KW_MODULE", "KW_OBJECT", "KW_TRUNCATABLE", 
			"KW_UNSIGNED", "KW_FIXED", "KW_UNION", "KW_ONEWAY", "KW_ANY", "KW_CHAR", 
			"KW_CASE", "KW_FLOAT", "KW_BOOLEAN", "KW_MULTIPLE", "KW_ABSTRACT", "KW_INOUT", 
			"KW_PROVIDES", "KW_CONSUMES", "KW_DOUBLE", "KW_TYPEPREFIX", "KW_TYPEID", 
			"KW_ATTRIBUTE", "KW_LOCAL", "KW_MANAGES", "KW_INTERFACE", "KW_COMPONENT", 
			"KW_MAP", "KW_BITFIELD", "KW_BITSET", "KW_BITMASK", "KW_INT8", "KW_UINT8", 
			"KW_INT16", "KW_UINT16", "KW_INT32", "KW_UINT32", "KW_INT64", "KW_UINT64", 
			"KW_AT_ANNOTATION", "ID", "WS", "COMMENT", "LINE_COMMENT", "VERSION_NUM", 
			"SHARP", "INCLUDE", "LINE", "PRAGMA", "PRAGMA_VERSION", "PRAGMA_PREFIX", 
			"PRAGMA_ID", "DEFINE", "DEFINED", "IF", "ELIF", "ELSE", "UNDEF", "IFDEF", 
			"IFNDEF", "ENDIF", "TRUE", "FALSE", "ERROR", "OP_BANG", "OP_LPAREN", 
			"OP_RPAREN", "OP_EQUAL", "OP_NOTEQUAL", "OP_AND", "OP_OR", "OP_LT", "OP_GT", 
			"OP_LE", "OP_GE", "DIRECTIVE_WHITESPACES", "DIRECTIVE_STRING", "CONDITIONAL_SYMBOL", 
			"DECIMAL_LITERAL", "FLOAT", "NEW_LINE", "DIRECTIVE_COMMENT", "DIRECTIVE_LINE_COMMENT", 
			"DIRECTIVE_NEW_LINE", "DIRECITVE_TEXT_NEW_LINE", "TEXT", "TEXT_NEW_LINE"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "IDLParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public IDLParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class SpecificationContext extends ParserRuleContext {
		public List<DefinitionContext> definition() {
			return getRuleContexts(DefinitionContext.class);
		}
		public DefinitionContext definition(int i) {
			return getRuleContext(DefinitionContext.class,i);
		}
		public SpecificationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_specification; }
	}

	public final SpecificationContext specification() throws RecognitionException {
		SpecificationContext _localctx = new SpecificationContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_specification);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(337); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(336);
				definition();
				}
				}
				setState(339); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << AT) | (1L << KW_TYPEDEF) | (1L << KW_CUSTOM) | (1L << KW_IMPORT) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_EVENTTYPE) | (1L << KW_ENUM))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (KW_HOME - 66)) | (1L << (KW_EXCEPTION - 66)) | (1L << (KW_CONST - 66)) | (1L << (KW_VALUETYPE - 66)) | (1L << (KW_MODULE - 66)) | (1L << (KW_UNION - 66)) | (1L << (KW_ABSTRACT - 66)) | (1L << (KW_TYPEPREFIX - 66)) | (1L << (KW_TYPEID - 66)) | (1L << (KW_LOCAL - 66)) | (1L << (KW_INTERFACE - 66)) | (1L << (KW_COMPONENT - 66)) | (1L << (KW_BITSET - 66)) | (1L << (KW_BITMASK - 66)) | (1L << (KW_AT_ANNOTATION - 66)) | (1L << (SHARP - 66)))) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DefinitionContext extends ParserRuleContext {
		public Type_declContext type_decl() {
			return getRuleContext(Type_declContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Const_declContext const_decl() {
			return getRuleContext(Const_declContext.class,0);
		}
		public Except_declContext except_decl() {
			return getRuleContext(Except_declContext.class,0);
		}
		public Interface_or_forward_declContext interface_or_forward_decl() {
			return getRuleContext(Interface_or_forward_declContext.class,0);
		}
		public ModuleContext module() {
			return getRuleContext(ModuleContext.class,0);
		}
		public ValueContext value() {
			return getRuleContext(ValueContext.class,0);
		}
		public Type_id_declContext type_id_decl() {
			return getRuleContext(Type_id_declContext.class,0);
		}
		public Type_prefix_declContext type_prefix_decl() {
			return getRuleContext(Type_prefix_declContext.class,0);
		}
		public EventContext event() {
			return getRuleContext(EventContext.class,0);
		}
		public ComponentContext component() {
			return getRuleContext(ComponentContext.class,0);
		}
		public Home_declContext home_decl() {
			return getRuleContext(Home_declContext.class,0);
		}
		public Annotation_declContext annotation_decl() {
			return getRuleContext(Annotation_declContext.class,0);
		}
		public IncludeContext include() {
			return getRuleContext(IncludeContext.class,0);
		}
		public TerminalNode SHARP() { return getToken(IDLParser.SHARP, 0); }
		public DirectiveContext directive() {
			return getRuleContext(DirectiveContext.class,0);
		}
		public TerminalNode NEW_LINE() { return getToken(IDLParser.NEW_LINE, 0); }
		public PragmaContext pragma() {
			return getRuleContext(PragmaContext.class,0);
		}
		public LineContext line() {
			return getRuleContext(LineContext.class,0);
		}
		public DefinitionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_definition; }
	}

	public final DefinitionContext definition() throws RecognitionException {
		DefinitionContext _localctx = new DefinitionContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_definition);
		int _la;
		try {
			setState(388);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,3,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(341);
				type_decl();
				setState(342);
				match(SEMICOLON);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(344);
				const_decl();
				setState(345);
				match(SEMICOLON);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(347);
				except_decl();
				setState(348);
				match(SEMICOLON);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(350);
				interface_or_forward_decl();
				setState(351);
				match(SEMICOLON);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(353);
				module();
				setState(354);
				match(SEMICOLON);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(356);
				value();
				setState(357);
				match(SEMICOLON);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(359);
				type_id_decl();
				setState(361);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMICOLON) {
					{
					setState(360);
					match(SEMICOLON);
					}
				}

				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(363);
				type_prefix_decl();
				setState(365);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMICOLON) {
					{
					setState(364);
					match(SEMICOLON);
					}
				}

				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(367);
				event();
				setState(368);
				match(SEMICOLON);
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(370);
				component();
				setState(371);
				match(SEMICOLON);
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(373);
				home_decl();
				setState(374);
				match(SEMICOLON);
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(376);
				annotation_decl();
				setState(377);
				match(SEMICOLON);
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(379);
				include();
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(380);
				match(SHARP);
				setState(381);
				directive();
				setState(382);
				match(NEW_LINE);
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(384);
				match(SHARP);
				setState(385);
				pragma();
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(386);
				match(SHARP);
				setState(387);
				line();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ModuleContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_MODULE() { return getToken(IDLParser.KW_MODULE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<DefinitionContext> definition() {
			return getRuleContexts(DefinitionContext.class);
		}
		public DefinitionContext definition(int i) {
			return getRuleContext(DefinitionContext.class,i);
		}
		public ModuleContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_module; }
	}

	public final ModuleContext module() throws RecognitionException {
		ModuleContext _localctx = new ModuleContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_module);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(390);
			annapps();
			setState(391);
			match(KW_MODULE);
			setState(392);
			match(ID);
			setState(393);
			match(LEFT_BRACE);
			setState(395); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(394);
				definition();
				}
				}
				setState(397); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << AT) | (1L << KW_TYPEDEF) | (1L << KW_CUSTOM) | (1L << KW_IMPORT) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_EVENTTYPE) | (1L << KW_ENUM))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (KW_HOME - 66)) | (1L << (KW_EXCEPTION - 66)) | (1L << (KW_CONST - 66)) | (1L << (KW_VALUETYPE - 66)) | (1L << (KW_MODULE - 66)) | (1L << (KW_UNION - 66)) | (1L << (KW_ABSTRACT - 66)) | (1L << (KW_TYPEPREFIX - 66)) | (1L << (KW_TYPEID - 66)) | (1L << (KW_LOCAL - 66)) | (1L << (KW_INTERFACE - 66)) | (1L << (KW_COMPONENT - 66)) | (1L << (KW_BITSET - 66)) | (1L << (KW_BITMASK - 66)) | (1L << (KW_AT_ANNOTATION - 66)) | (1L << (SHARP - 66)))) != 0) );
			setState(399);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_or_forward_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Interface_declContext interface_decl() {
			return getRuleContext(Interface_declContext.class,0);
		}
		public Forward_declContext forward_decl() {
			return getRuleContext(Forward_declContext.class,0);
		}
		public Interface_or_forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_or_forward_decl; }
	}

	public final Interface_or_forward_declContext interface_or_forward_decl() throws RecognitionException {
		Interface_or_forward_declContext _localctx = new Interface_or_forward_declContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_interface_or_forward_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(401);
			annapps();
			setState(404);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,5,_ctx) ) {
			case 1:
				{
				setState(402);
				interface_decl();
				}
				break;
			case 2:
				{
				setState(403);
				forward_decl();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_declContext extends ParserRuleContext {
		public Interface_headerContext interface_header() {
			return getRuleContext(Interface_headerContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public Interface_bodyContext interface_body() {
			return getRuleContext(Interface_bodyContext.class,0);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public Interface_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_decl; }
	}

	public final Interface_declContext interface_decl() throws RecognitionException {
		Interface_declContext _localctx = new Interface_declContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_interface_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(406);
			interface_header();
			setState(407);
			match(LEFT_BRACE);
			setState(408);
			interface_body();
			setState(409);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Forward_declContext extends ParserRuleContext {
		public TerminalNode KW_INTERFACE() { return getToken(IDLParser.KW_INTERFACE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public TerminalNode KW_LOCAL() { return getToken(IDLParser.KW_LOCAL, 0); }
		public Forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_forward_decl; }
	}

	public final Forward_declContext forward_decl() throws RecognitionException {
		Forward_declContext _localctx = new Forward_declContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_forward_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(412);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_ABSTRACT || _la==KW_LOCAL) {
				{
				setState(411);
				_la = _input.LA(1);
				if ( !(_la==KW_ABSTRACT || _la==KW_LOCAL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(414);
			match(KW_INTERFACE);
			setState(415);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_headerContext extends ParserRuleContext {
		public TerminalNode KW_INTERFACE() { return getToken(IDLParser.KW_INTERFACE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Interface_inheritance_specContext interface_inheritance_spec() {
			return getRuleContext(Interface_inheritance_specContext.class,0);
		}
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public TerminalNode KW_LOCAL() { return getToken(IDLParser.KW_LOCAL, 0); }
		public Interface_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_header; }
	}

	public final Interface_headerContext interface_header() throws RecognitionException {
		Interface_headerContext _localctx = new Interface_headerContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_interface_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(418);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_ABSTRACT || _la==KW_LOCAL) {
				{
				setState(417);
				_la = _input.LA(1);
				if ( !(_la==KW_ABSTRACT || _la==KW_LOCAL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(420);
			match(KW_INTERFACE);
			setState(421);
			match(ID);
			setState(423);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(422);
				interface_inheritance_spec();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_bodyContext extends ParserRuleContext {
		public List<Interface_exportContext> interface_export() {
			return getRuleContexts(Interface_exportContext.class);
		}
		public Interface_exportContext interface_export(int i) {
			return getRuleContext(Interface_exportContext.class,i);
		}
		public Interface_bodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_body; }
	}

	public final Interface_bodyContext interface_body() throws RecognitionException {
		Interface_bodyContext _localctx = new Interface_bodyContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_interface_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(428);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_VOID) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(425);
				interface_export();
				}
				}
				setState(430);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_exportContext extends ParserRuleContext {
		public Type_declContext type_decl() {
			return getRuleContext(Type_declContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Const_declContext const_decl() {
			return getRuleContext(Const_declContext.class,0);
		}
		public Except_declContext except_decl() {
			return getRuleContext(Except_declContext.class,0);
		}
		public Attr_declContext attr_decl() {
			return getRuleContext(Attr_declContext.class,0);
		}
		public Op_declContext op_decl() {
			return getRuleContext(Op_declContext.class,0);
		}
		public Type_id_declContext type_id_decl() {
			return getRuleContext(Type_id_declContext.class,0);
		}
		public Type_prefix_declContext type_prefix_decl() {
			return getRuleContext(Type_prefix_declContext.class,0);
		}
		public Interface_exportContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_export; }
	}

	public final Interface_exportContext interface_export() throws RecognitionException {
		Interface_exportContext _localctx = new Interface_exportContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_interface_export);
		int _la;
		try {
			setState(454);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(431);
				type_decl();
				setState(432);
				match(SEMICOLON);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(434);
				const_decl();
				setState(435);
				match(SEMICOLON);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(437);
				except_decl();
				setState(438);
				match(SEMICOLON);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(440);
				attr_decl();
				setState(441);
				match(SEMICOLON);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(443);
				op_decl();
				setState(444);
				match(SEMICOLON);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(446);
				type_id_decl();
				setState(448);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMICOLON) {
					{
					setState(447);
					match(SEMICOLON);
					}
				}

				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(450);
				type_prefix_decl();
				setState(452);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMICOLON) {
					{
					setState(451);
					match(SEMICOLON);
					}
				}

				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_inheritance_specContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public List<Interface_nameContext> interface_name() {
			return getRuleContexts(Interface_nameContext.class);
		}
		public Interface_nameContext interface_name(int i) {
			return getRuleContext(Interface_nameContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Interface_inheritance_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_inheritance_spec; }
	}

	public final Interface_inheritance_specContext interface_inheritance_spec() throws RecognitionException {
		Interface_inheritance_specContext _localctx = new Interface_inheritance_specContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_interface_inheritance_spec);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(456);
			match(COLON);
			setState(457);
			interface_name();
			setState(462);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(458);
				match(COMMA);
				setState(459);
				interface_name();
				}
				}
				setState(464);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_nameContext extends ParserRuleContext {
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Interface_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_name; }
	}

	public final Interface_nameContext interface_name() throws RecognitionException {
		Interface_nameContext _localctx = new Interface_nameContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_interface_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(465);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Scoped_nameContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(IDLParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(IDLParser.ID, i);
		}
		public List<TerminalNode> DOUBLE_COLON() { return getTokens(IDLParser.DOUBLE_COLON); }
		public TerminalNode DOUBLE_COLON(int i) {
			return getToken(IDLParser.DOUBLE_COLON, i);
		}
		public Scoped_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_scoped_name; }
	}

	public final Scoped_nameContext scoped_name() throws RecognitionException {
		Scoped_nameContext _localctx = new Scoped_nameContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_scoped_name);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(468);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==DOUBLE_COLON) {
				{
				setState(467);
				match(DOUBLE_COLON);
				}
			}

			setState(470);
			match(ID);
			setState(475);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(471);
					match(DOUBLE_COLON);
					setState(472);
					match(ID);
					}
					} 
				}
				setState(477);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ValueContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Value_declContext value_decl() {
			return getRuleContext(Value_declContext.class,0);
		}
		public Value_abs_declContext value_abs_decl() {
			return getRuleContext(Value_abs_declContext.class,0);
		}
		public Value_box_declContext value_box_decl() {
			return getRuleContext(Value_box_declContext.class,0);
		}
		public Value_forward_declContext value_forward_decl() {
			return getRuleContext(Value_forward_declContext.class,0);
		}
		public ValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value; }
	}

	public final ValueContext value() throws RecognitionException {
		ValueContext _localctx = new ValueContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_value);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(478);
			annapps();
			setState(483);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,16,_ctx) ) {
			case 1:
				{
				setState(479);
				value_decl();
				}
				break;
			case 2:
				{
				setState(480);
				value_abs_decl();
				}
				break;
			case 3:
				{
				setState(481);
				value_box_decl();
				}
				break;
			case 4:
				{
				setState(482);
				value_forward_decl();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_forward_declContext extends ParserRuleContext {
		public TerminalNode KW_VALUETYPE() { return getToken(IDLParser.KW_VALUETYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public Value_forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_forward_decl; }
	}

	public final Value_forward_declContext value_forward_decl() throws RecognitionException {
		Value_forward_declContext _localctx = new Value_forward_declContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_value_forward_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(486);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_ABSTRACT) {
				{
				setState(485);
				match(KW_ABSTRACT);
				}
			}

			setState(488);
			match(KW_VALUETYPE);
			setState(489);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_box_declContext extends ParserRuleContext {
		public TerminalNode KW_VALUETYPE() { return getToken(IDLParser.KW_VALUETYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Type_specContext type_spec() {
			return getRuleContext(Type_specContext.class,0);
		}
		public Value_box_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_box_decl; }
	}

	public final Value_box_declContext value_box_decl() throws RecognitionException {
		Value_box_declContext _localctx = new Value_box_declContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_value_box_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(491);
			match(KW_VALUETYPE);
			setState(492);
			match(ID);
			setState(493);
			type_spec();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_abs_declContext extends ParserRuleContext {
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public TerminalNode KW_VALUETYPE() { return getToken(IDLParser.KW_VALUETYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Value_inheritance_specContext value_inheritance_spec() {
			return getRuleContext(Value_inheritance_specContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<Interface_exportContext> interface_export() {
			return getRuleContexts(Interface_exportContext.class);
		}
		public Interface_exportContext interface_export(int i) {
			return getRuleContext(Interface_exportContext.class,i);
		}
		public Value_abs_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_abs_decl; }
	}

	public final Value_abs_declContext value_abs_decl() throws RecognitionException {
		Value_abs_declContext _localctx = new Value_abs_declContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_value_abs_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(495);
			match(KW_ABSTRACT);
			setState(496);
			match(KW_VALUETYPE);
			setState(497);
			match(ID);
			setState(498);
			value_inheritance_spec();
			setState(499);
			match(LEFT_BRACE);
			setState(503);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_VOID) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(500);
				interface_export();
				}
				}
				setState(505);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(506);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_declContext extends ParserRuleContext {
		public Value_headerContext value_header() {
			return getRuleContext(Value_headerContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<Value_elementContext> value_element() {
			return getRuleContexts(Value_elementContext.class);
		}
		public Value_elementContext value_element(int i) {
			return getRuleContext(Value_elementContext.class,i);
		}
		public Value_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_decl; }
	}

	public final Value_declContext value_decl() throws RecognitionException {
		Value_declContext _localctx = new Value_declContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_value_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(508);
			value_header();
			setState(509);
			match(LEFT_BRACE);
			setState(513);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_VOID) | (1L << KW_PRIVATE) | (1L << KW_WCHAR) | (1L << KW_PUBLIC) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_FACTORY - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(510);
				value_element();
				}
				}
				setState(515);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(516);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_headerContext extends ParserRuleContext {
		public TerminalNode KW_VALUETYPE() { return getToken(IDLParser.KW_VALUETYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Value_inheritance_specContext value_inheritance_spec() {
			return getRuleContext(Value_inheritance_specContext.class,0);
		}
		public TerminalNode KW_CUSTOM() { return getToken(IDLParser.KW_CUSTOM, 0); }
		public Value_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_header; }
	}

	public final Value_headerContext value_header() throws RecognitionException {
		Value_headerContext _localctx = new Value_headerContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_value_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(519);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_CUSTOM) {
				{
				setState(518);
				match(KW_CUSTOM);
				}
			}

			setState(521);
			match(KW_VALUETYPE);
			setState(522);
			match(ID);
			setState(523);
			value_inheritance_spec();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_inheritance_specContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public List<Value_nameContext> value_name() {
			return getRuleContexts(Value_nameContext.class);
		}
		public Value_nameContext value_name(int i) {
			return getRuleContext(Value_nameContext.class,i);
		}
		public TerminalNode KW_SUPPORTS() { return getToken(IDLParser.KW_SUPPORTS, 0); }
		public List<Interface_nameContext> interface_name() {
			return getRuleContexts(Interface_nameContext.class);
		}
		public Interface_nameContext interface_name(int i) {
			return getRuleContext(Interface_nameContext.class,i);
		}
		public TerminalNode KW_TRUNCATABLE() { return getToken(IDLParser.KW_TRUNCATABLE, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Value_inheritance_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_inheritance_spec; }
	}

	public final Value_inheritance_specContext value_inheritance_spec() throws RecognitionException {
		Value_inheritance_specContext _localctx = new Value_inheritance_specContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_value_inheritance_spec);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(537);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(525);
				match(COLON);
				setState(527);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==KW_TRUNCATABLE) {
					{
					setState(526);
					match(KW_TRUNCATABLE);
					}
				}

				setState(529);
				value_name();
				setState(534);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(530);
					match(COMMA);
					setState(531);
					value_name();
					}
					}
					setState(536);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(548);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_SUPPORTS) {
				{
				setState(539);
				match(KW_SUPPORTS);
				setState(540);
				interface_name();
				setState(545);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(541);
					match(COMMA);
					setState(542);
					interface_name();
					}
					}
					setState(547);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_nameContext extends ParserRuleContext {
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Value_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_name; }
	}

	public final Value_nameContext value_name() throws RecognitionException {
		Value_nameContext _localctx = new Value_nameContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_value_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(550);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_elementContext extends ParserRuleContext {
		public Interface_exportContext interface_export() {
			return getRuleContext(Interface_exportContext.class,0);
		}
		public State_memberContext state_member() {
			return getRuleContext(State_memberContext.class,0);
		}
		public Init_declContext init_decl() {
			return getRuleContext(Init_declContext.class,0);
		}
		public Value_elementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_element; }
	}

	public final Value_elementContext value_element() throws RecognitionException {
		Value_elementContext _localctx = new Value_elementContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_value_element);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(555);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,26,_ctx) ) {
			case 1:
				{
				setState(552);
				interface_export();
				}
				break;
			case 2:
				{
				setState(553);
				state_member();
				}
				break;
			case 3:
				{
				setState(554);
				init_decl();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class State_memberContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Type_specContext type_spec() {
			return getRuleContext(Type_specContext.class,0);
		}
		public DeclaratorsContext declarators() {
			return getRuleContext(DeclaratorsContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public TerminalNode KW_PUBLIC() { return getToken(IDLParser.KW_PUBLIC, 0); }
		public TerminalNode KW_PRIVATE() { return getToken(IDLParser.KW_PRIVATE, 0); }
		public State_memberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_state_member; }
	}

	public final State_memberContext state_member() throws RecognitionException {
		State_memberContext _localctx = new State_memberContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_state_member);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(557);
			annapps();
			setState(558);
			_la = _input.LA(1);
			if ( !(_la==KW_PRIVATE || _la==KW_PUBLIC) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(559);
			type_spec();
			setState(560);
			declarators();
			setState(561);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Init_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_FACTORY() { return getToken(IDLParser.KW_FACTORY, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Init_param_declsContext init_param_decls() {
			return getRuleContext(Init_param_declsContext.class,0);
		}
		public Raises_exprContext raises_expr() {
			return getRuleContext(Raises_exprContext.class,0);
		}
		public Init_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_init_decl; }
	}

	public final Init_declContext init_decl() throws RecognitionException {
		Init_declContext _localctx = new Init_declContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_init_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(563);
			annapps();
			setState(564);
			match(KW_FACTORY);
			setState(565);
			match(ID);
			setState(566);
			match(LEFT_BRACKET);
			setState(568);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AT || _la==KW_IN) {
				{
				setState(567);
				init_param_decls();
				}
			}

			setState(570);
			match(RIGHT_BRACKET);
			setState(572);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_RAISES) {
				{
				setState(571);
				raises_expr();
				}
			}

			setState(574);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Init_param_declsContext extends ParserRuleContext {
		public List<Init_param_declContext> init_param_decl() {
			return getRuleContexts(Init_param_declContext.class);
		}
		public Init_param_declContext init_param_decl(int i) {
			return getRuleContext(Init_param_declContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Init_param_declsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_init_param_decls; }
	}

	public final Init_param_declsContext init_param_decls() throws RecognitionException {
		Init_param_declsContext _localctx = new Init_param_declsContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_init_param_decls);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(576);
			init_param_decl();
			setState(581);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(577);
				match(COMMA);
				setState(578);
				init_param_decl();
				}
				}
				setState(583);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Init_param_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Init_param_attributeContext init_param_attribute() {
			return getRuleContext(Init_param_attributeContext.class,0);
		}
		public Param_type_specContext param_type_spec() {
			return getRuleContext(Param_type_specContext.class,0);
		}
		public Simple_declaratorContext simple_declarator() {
			return getRuleContext(Simple_declaratorContext.class,0);
		}
		public Init_param_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_init_param_decl; }
	}

	public final Init_param_declContext init_param_decl() throws RecognitionException {
		Init_param_declContext _localctx = new Init_param_declContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_init_param_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(584);
			annapps();
			setState(585);
			init_param_attribute();
			setState(586);
			param_type_spec();
			setState(587);
			simple_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Init_param_attributeContext extends ParserRuleContext {
		public TerminalNode KW_IN() { return getToken(IDLParser.KW_IN, 0); }
		public Init_param_attributeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_init_param_attribute; }
	}

	public final Init_param_attributeContext init_param_attribute() throws RecognitionException {
		Init_param_attributeContext _localctx = new Init_param_attributeContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_init_param_attribute);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(589);
			match(KW_IN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Const_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_CONST() { return getToken(IDLParser.KW_CONST, 0); }
		public Const_typeContext const_type() {
			return getRuleContext(Const_typeContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode EQUAL() { return getToken(IDLParser.EQUAL, 0); }
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public Const_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_const_decl; }
	}

	public final Const_declContext const_decl() throws RecognitionException {
		Const_declContext _localctx = new Const_declContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_const_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(591);
			annapps();
			setState(592);
			match(KW_CONST);
			setState(593);
			const_type();
			setState(594);
			match(ID);
			setState(595);
			match(EQUAL);
			setState(596);
			const_exp();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Const_typeContext extends ParserRuleContext {
		public Integer_typeContext integer_type() {
			return getRuleContext(Integer_typeContext.class,0);
		}
		public Char_typeContext char_type() {
			return getRuleContext(Char_typeContext.class,0);
		}
		public Wide_char_typeContext wide_char_type() {
			return getRuleContext(Wide_char_typeContext.class,0);
		}
		public Boolean_typeContext boolean_type() {
			return getRuleContext(Boolean_typeContext.class,0);
		}
		public Floating_pt_typeContext floating_pt_type() {
			return getRuleContext(Floating_pt_typeContext.class,0);
		}
		public String_typeContext string_type() {
			return getRuleContext(String_typeContext.class,0);
		}
		public Wide_string_typeContext wide_string_type() {
			return getRuleContext(Wide_string_typeContext.class,0);
		}
		public Fixed_pt_const_typeContext fixed_pt_const_type() {
			return getRuleContext(Fixed_pt_const_typeContext.class,0);
		}
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Octet_typeContext octet_type() {
			return getRuleContext(Octet_typeContext.class,0);
		}
		public Const_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_const_type; }
	}

	public final Const_typeContext const_type() throws RecognitionException {
		Const_typeContext _localctx = new Const_typeContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_const_type);
		try {
			setState(608);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,30,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(598);
				integer_type();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(599);
				char_type();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(600);
				wide_char_type();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(601);
				boolean_type();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(602);
				floating_pt_type();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(603);
				string_type();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(604);
				wide_string_type();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(605);
				fixed_pt_const_type();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(606);
				scoped_name();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(607);
				octet_type();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Const_expContext extends ParserRuleContext {
		public Or_exprContext or_expr() {
			return getRuleContext(Or_exprContext.class,0);
		}
		public Const_expContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_const_exp; }
	}

	public final Const_expContext const_exp() throws RecognitionException {
		Const_expContext _localctx = new Const_expContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_const_exp);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(610);
			or_expr();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Or_exprContext extends ParserRuleContext {
		public List<Xor_exprContext> xor_expr() {
			return getRuleContexts(Xor_exprContext.class);
		}
		public Xor_exprContext xor_expr(int i) {
			return getRuleContext(Xor_exprContext.class,i);
		}
		public List<TerminalNode> PIPE() { return getTokens(IDLParser.PIPE); }
		public TerminalNode PIPE(int i) {
			return getToken(IDLParser.PIPE, i);
		}
		public Or_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_or_expr; }
	}

	public final Or_exprContext or_expr() throws RecognitionException {
		Or_exprContext _localctx = new Or_exprContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_or_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(612);
			xor_expr();
			setState(617);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==PIPE) {
				{
				{
				setState(613);
				match(PIPE);
				setState(614);
				xor_expr();
				}
				}
				setState(619);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Xor_exprContext extends ParserRuleContext {
		public List<And_exprContext> and_expr() {
			return getRuleContexts(And_exprContext.class);
		}
		public And_exprContext and_expr(int i) {
			return getRuleContext(And_exprContext.class,i);
		}
		public List<TerminalNode> CARET() { return getTokens(IDLParser.CARET); }
		public TerminalNode CARET(int i) {
			return getToken(IDLParser.CARET, i);
		}
		public Xor_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_xor_expr; }
	}

	public final Xor_exprContext xor_expr() throws RecognitionException {
		Xor_exprContext _localctx = new Xor_exprContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_xor_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(620);
			and_expr();
			setState(625);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CARET) {
				{
				{
				setState(621);
				match(CARET);
				setState(622);
				and_expr();
				}
				}
				setState(627);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class And_exprContext extends ParserRuleContext {
		public List<Shift_exprContext> shift_expr() {
			return getRuleContexts(Shift_exprContext.class);
		}
		public Shift_exprContext shift_expr(int i) {
			return getRuleContext(Shift_exprContext.class,i);
		}
		public List<TerminalNode> AMPERSAND() { return getTokens(IDLParser.AMPERSAND); }
		public TerminalNode AMPERSAND(int i) {
			return getToken(IDLParser.AMPERSAND, i);
		}
		public And_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_and_expr; }
	}

	public final And_exprContext and_expr() throws RecognitionException {
		And_exprContext _localctx = new And_exprContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_and_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(628);
			shift_expr();
			setState(633);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==AMPERSAND) {
				{
				{
				setState(629);
				match(AMPERSAND);
				setState(630);
				shift_expr();
				}
				}
				setState(635);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Shift_exprContext extends ParserRuleContext {
		public List<Add_exprContext> add_expr() {
			return getRuleContexts(Add_exprContext.class);
		}
		public Add_exprContext add_expr(int i) {
			return getRuleContext(Add_exprContext.class,i);
		}
		public List<TerminalNode> RIGHT_SHIFT() { return getTokens(IDLParser.RIGHT_SHIFT); }
		public TerminalNode RIGHT_SHIFT(int i) {
			return getToken(IDLParser.RIGHT_SHIFT, i);
		}
		public List<TerminalNode> LEFT_SHIFT() { return getTokens(IDLParser.LEFT_SHIFT); }
		public TerminalNode LEFT_SHIFT(int i) {
			return getToken(IDLParser.LEFT_SHIFT, i);
		}
		public Shift_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_shift_expr; }
	}

	public final Shift_exprContext shift_expr() throws RecognitionException {
		Shift_exprContext _localctx = new Shift_exprContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_shift_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(636);
			add_expr();
			setState(641);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==RIGHT_SHIFT || _la==LEFT_SHIFT) {
				{
				{
				setState(637);
				_la = _input.LA(1);
				if ( !(_la==RIGHT_SHIFT || _la==LEFT_SHIFT) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(638);
				add_expr();
				}
				}
				setState(643);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Add_exprContext extends ParserRuleContext {
		public List<Mult_exprContext> mult_expr() {
			return getRuleContexts(Mult_exprContext.class);
		}
		public Mult_exprContext mult_expr(int i) {
			return getRuleContext(Mult_exprContext.class,i);
		}
		public List<TerminalNode> PLUS() { return getTokens(IDLParser.PLUS); }
		public TerminalNode PLUS(int i) {
			return getToken(IDLParser.PLUS, i);
		}
		public List<TerminalNode> MINUS() { return getTokens(IDLParser.MINUS); }
		public TerminalNode MINUS(int i) {
			return getToken(IDLParser.MINUS, i);
		}
		public Add_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_add_expr; }
	}

	public final Add_exprContext add_expr() throws RecognitionException {
		Add_exprContext _localctx = new Add_exprContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_add_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(644);
			mult_expr();
			setState(649);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==PLUS || _la==MINUS) {
				{
				{
				setState(645);
				_la = _input.LA(1);
				if ( !(_la==PLUS || _la==MINUS) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(646);
				mult_expr();
				}
				}
				setState(651);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Mult_exprContext extends ParserRuleContext {
		public List<Unary_exprContext> unary_expr() {
			return getRuleContexts(Unary_exprContext.class);
		}
		public Unary_exprContext unary_expr(int i) {
			return getRuleContext(Unary_exprContext.class,i);
		}
		public List<TerminalNode> STAR() { return getTokens(IDLParser.STAR); }
		public TerminalNode STAR(int i) {
			return getToken(IDLParser.STAR, i);
		}
		public List<TerminalNode> SLASH() { return getTokens(IDLParser.SLASH); }
		public TerminalNode SLASH(int i) {
			return getToken(IDLParser.SLASH, i);
		}
		public List<TerminalNode> PERCENT() { return getTokens(IDLParser.PERCENT); }
		public TerminalNode PERCENT(int i) {
			return getToken(IDLParser.PERCENT, i);
		}
		public Mult_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_mult_expr; }
	}

	public final Mult_exprContext mult_expr() throws RecognitionException {
		Mult_exprContext _localctx = new Mult_exprContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_mult_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(652);
			unary_expr();
			setState(657);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << SLASH) | (1L << STAR) | (1L << PERCENT))) != 0)) {
				{
				{
				setState(653);
				_la = _input.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << SLASH) | (1L << STAR) | (1L << PERCENT))) != 0)) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(654);
				unary_expr();
				}
				}
				setState(659);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unary_exprContext extends ParserRuleContext {
		public Unary_operatorContext unary_operator() {
			return getRuleContext(Unary_operatorContext.class,0);
		}
		public Primary_exprContext primary_expr() {
			return getRuleContext(Primary_exprContext.class,0);
		}
		public Unary_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unary_expr; }
	}

	public final Unary_exprContext unary_expr() throws RecognitionException {
		Unary_exprContext _localctx = new Unary_exprContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_unary_expr);
		try {
			setState(664);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case TILDE:
			case PLUS:
			case MINUS:
				enterOuterAlt(_localctx, 1);
				{
				setState(660);
				unary_operator();
				setState(661);
				primary_expr();
				}
				break;
			case INTEGER_LITERAL:
			case OCTAL_LITERAL:
			case HEX_LITERAL:
			case FLOATING_PT_LITERAL:
			case FIXED_PT_LITERAL:
			case WIDE_CHARACTER_LITERAL:
			case CHARACTER_LITERAL:
			case WIDE_STRING_LITERAL:
			case STRING_LITERAL:
			case BOOLEAN_LITERAL:
			case LEFT_BRACKET:
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(663);
				primary_expr();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unary_operatorContext extends ParserRuleContext {
		public TerminalNode MINUS() { return getToken(IDLParser.MINUS, 0); }
		public TerminalNode PLUS() { return getToken(IDLParser.PLUS, 0); }
		public TerminalNode TILDE() { return getToken(IDLParser.TILDE, 0); }
		public Unary_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unary_operator; }
	}

	public final Unary_operatorContext unary_operator() throws RecognitionException {
		Unary_operatorContext _localctx = new Unary_operatorContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_unary_operator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(666);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << TILDE) | (1L << PLUS) | (1L << MINUS))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Primary_exprContext extends ParserRuleContext {
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public Primary_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primary_expr; }
	}

	public final Primary_exprContext primary_expr() throws RecognitionException {
		Primary_exprContext _localctx = new Primary_exprContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_primary_expr);
		try {
			setState(674);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(668);
				scoped_name();
				}
				break;
			case INTEGER_LITERAL:
			case OCTAL_LITERAL:
			case HEX_LITERAL:
			case FLOATING_PT_LITERAL:
			case FIXED_PT_LITERAL:
			case WIDE_CHARACTER_LITERAL:
			case CHARACTER_LITERAL:
			case WIDE_STRING_LITERAL:
			case STRING_LITERAL:
			case BOOLEAN_LITERAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(669);
				literal();
				}
				break;
			case LEFT_BRACKET:
				enterOuterAlt(_localctx, 3);
				{
				setState(670);
				match(LEFT_BRACKET);
				setState(671);
				const_exp();
				setState(672);
				match(RIGHT_BRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LiteralContext extends ParserRuleContext {
		public TerminalNode HEX_LITERAL() { return getToken(IDLParser.HEX_LITERAL, 0); }
		public TerminalNode INTEGER_LITERAL() { return getToken(IDLParser.INTEGER_LITERAL, 0); }
		public TerminalNode OCTAL_LITERAL() { return getToken(IDLParser.OCTAL_LITERAL, 0); }
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public TerminalNode WIDE_STRING_LITERAL() { return getToken(IDLParser.WIDE_STRING_LITERAL, 0); }
		public TerminalNode CHARACTER_LITERAL() { return getToken(IDLParser.CHARACTER_LITERAL, 0); }
		public TerminalNode WIDE_CHARACTER_LITERAL() { return getToken(IDLParser.WIDE_CHARACTER_LITERAL, 0); }
		public TerminalNode FIXED_PT_LITERAL() { return getToken(IDLParser.FIXED_PT_LITERAL, 0); }
		public TerminalNode FLOATING_PT_LITERAL() { return getToken(IDLParser.FLOATING_PT_LITERAL, 0); }
		public TerminalNode BOOLEAN_LITERAL() { return getToken(IDLParser.BOOLEAN_LITERAL, 0); }
		public LiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literal; }
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(676);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << INTEGER_LITERAL) | (1L << OCTAL_LITERAL) | (1L << HEX_LITERAL) | (1L << FLOATING_PT_LITERAL) | (1L << FIXED_PT_LITERAL) | (1L << WIDE_CHARACTER_LITERAL) | (1L << CHARACTER_LITERAL) | (1L << WIDE_STRING_LITERAL) | (1L << STRING_LITERAL) | (1L << BOOLEAN_LITERAL))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Positive_int_constContext extends ParserRuleContext {
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public Positive_int_constContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_positive_int_const; }
	}

	public final Positive_int_constContext positive_int_const() throws RecognitionException {
		Positive_int_constContext _localctx = new Positive_int_constContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_positive_int_const);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(678);
			const_exp();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_declContext extends ParserRuleContext {
		public Type_defContext type_def() {
			return getRuleContext(Type_defContext.class,0);
		}
		public Struct_typeContext struct_type() {
			return getRuleContext(Struct_typeContext.class,0);
		}
		public Union_typeContext union_type() {
			return getRuleContext(Union_typeContext.class,0);
		}
		public Enum_typeContext enum_type() {
			return getRuleContext(Enum_typeContext.class,0);
		}
		public Bitset_typeContext bitset_type() {
			return getRuleContext(Bitset_typeContext.class,0);
		}
		public Bitmask_typeContext bitmask_type() {
			return getRuleContext(Bitmask_typeContext.class,0);
		}
		public Native_typeContext native_type() {
			return getRuleContext(Native_typeContext.class,0);
		}
		public Constr_forward_declContext constr_forward_decl() {
			return getRuleContext(Constr_forward_declContext.class,0);
		}
		public Type_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_decl; }
	}

	public final Type_declContext type_decl() throws RecognitionException {
		Type_declContext _localctx = new Type_declContext(_ctx, getState());
		enterRule(_localctx, 80, RULE_type_decl);
		try {
			setState(688);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(680);
				type_def();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(681);
				struct_type();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(682);
				union_type();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(683);
				enum_type();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(684);
				bitset_type();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(685);
				bitmask_type();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(686);
				native_type();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(687);
				constr_forward_decl();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_defContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_TYPEDEF() { return getToken(IDLParser.KW_TYPEDEF, 0); }
		public Type_declaratorContext type_declarator() {
			return getRuleContext(Type_declaratorContext.class,0);
		}
		public Type_defContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_def; }
	}

	public final Type_defContext type_def() throws RecognitionException {
		Type_defContext _localctx = new Type_defContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_type_def);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(690);
			annapps();
			setState(691);
			match(KW_TYPEDEF);
			setState(692);
			type_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Native_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_NATIVE() { return getToken(IDLParser.KW_NATIVE, 0); }
		public Simple_declaratorsContext simple_declarators() {
			return getRuleContext(Simple_declaratorsContext.class,0);
		}
		public Native_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_native_type; }
	}

	public final Native_typeContext native_type() throws RecognitionException {
		Native_typeContext _localctx = new Native_typeContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_native_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(694);
			annapps();
			setState(695);
			match(KW_NATIVE);
			setState(696);
			simple_declarators();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_declaratorContext extends ParserRuleContext {
		public Type_specContext type_spec() {
			return getRuleContext(Type_specContext.class,0);
		}
		public DeclaratorsContext declarators() {
			return getRuleContext(DeclaratorsContext.class,0);
		}
		public Type_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_declarator; }
	}

	public final Type_declaratorContext type_declarator() throws RecognitionException {
		Type_declaratorContext _localctx = new Type_declaratorContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_type_declarator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(698);
			type_spec();
			setState(699);
			declarators();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_specContext extends ParserRuleContext {
		public Simple_type_specContext simple_type_spec() {
			return getRuleContext(Simple_type_specContext.class,0);
		}
		public Constr_type_specContext constr_type_spec() {
			return getRuleContext(Constr_type_specContext.class,0);
		}
		public Type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_spec; }
	}

	public final Type_specContext type_spec() throws RecognitionException {
		Type_specContext _localctx = new Type_specContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_type_spec);
		try {
			setState(703);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case KW_STRING:
			case KW_OCTET:
			case KW_SEQUENCE:
			case KW_WCHAR:
			case KW_SHORT:
			case KW_LONG:
			case KW_WSTRING:
			case KW_VALUEBASE:
			case KW_OBJECT:
			case KW_UNSIGNED:
			case KW_FIXED:
			case KW_ANY:
			case KW_CHAR:
			case KW_FLOAT:
			case KW_BOOLEAN:
			case KW_DOUBLE:
			case KW_MAP:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(701);
				simple_type_spec();
				}
				break;
			case AT:
			case KW_STRUCT:
			case KW_ENUM:
			case KW_UNION:
			case KW_BITSET:
			case KW_BITMASK:
				enterOuterAlt(_localctx, 2);
				{
				setState(702);
				constr_type_spec();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Simple_type_specContext extends ParserRuleContext {
		public Base_type_specContext base_type_spec() {
			return getRuleContext(Base_type_specContext.class,0);
		}
		public Template_type_specContext template_type_spec() {
			return getRuleContext(Template_type_specContext.class,0);
		}
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Simple_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simple_type_spec; }
	}

	public final Simple_type_specContext simple_type_spec() throws RecognitionException {
		Simple_type_specContext _localctx = new Simple_type_specContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_simple_type_spec);
		try {
			setState(708);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_OCTET:
			case KW_WCHAR:
			case KW_SHORT:
			case KW_LONG:
			case KW_VALUEBASE:
			case KW_OBJECT:
			case KW_UNSIGNED:
			case KW_ANY:
			case KW_CHAR:
			case KW_FLOAT:
			case KW_BOOLEAN:
			case KW_DOUBLE:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
				enterOuterAlt(_localctx, 1);
				{
				setState(705);
				base_type_spec();
				}
				break;
			case KW_STRING:
			case KW_SEQUENCE:
			case KW_WSTRING:
			case KW_FIXED:
			case KW_MAP:
				enterOuterAlt(_localctx, 2);
				{
				setState(706);
				template_type_spec();
				}
				break;
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 3);
				{
				setState(707);
				scoped_name();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bitfield_type_specContext extends ParserRuleContext {
		public Integer_typeContext integer_type() {
			return getRuleContext(Integer_typeContext.class,0);
		}
		public Boolean_typeContext boolean_type() {
			return getRuleContext(Boolean_typeContext.class,0);
		}
		public Octet_typeContext octet_type() {
			return getRuleContext(Octet_typeContext.class,0);
		}
		public Bitfield_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitfield_type_spec; }
	}

	public final Bitfield_type_specContext bitfield_type_spec() throws RecognitionException {
		Bitfield_type_specContext _localctx = new Bitfield_type_specContext(_ctx, getState());
		enterRule(_localctx, 92, RULE_bitfield_type_spec);
		try {
			setState(713);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_SHORT:
			case KW_LONG:
			case KW_UNSIGNED:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
				enterOuterAlt(_localctx, 1);
				{
				setState(710);
				integer_type();
				}
				break;
			case KW_BOOLEAN:
				enterOuterAlt(_localctx, 2);
				{
				setState(711);
				boolean_type();
				}
				break;
			case KW_OCTET:
				enterOuterAlt(_localctx, 3);
				{
				setState(712);
				octet_type();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Base_type_specContext extends ParserRuleContext {
		public Floating_pt_typeContext floating_pt_type() {
			return getRuleContext(Floating_pt_typeContext.class,0);
		}
		public Integer_typeContext integer_type() {
			return getRuleContext(Integer_typeContext.class,0);
		}
		public Char_typeContext char_type() {
			return getRuleContext(Char_typeContext.class,0);
		}
		public Wide_char_typeContext wide_char_type() {
			return getRuleContext(Wide_char_typeContext.class,0);
		}
		public Boolean_typeContext boolean_type() {
			return getRuleContext(Boolean_typeContext.class,0);
		}
		public Octet_typeContext octet_type() {
			return getRuleContext(Octet_typeContext.class,0);
		}
		public Any_typeContext any_type() {
			return getRuleContext(Any_typeContext.class,0);
		}
		public Object_typeContext object_type() {
			return getRuleContext(Object_typeContext.class,0);
		}
		public Value_base_typeContext value_base_type() {
			return getRuleContext(Value_base_typeContext.class,0);
		}
		public Base_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_base_type_spec; }
	}

	public final Base_type_specContext base_type_spec() throws RecognitionException {
		Base_type_specContext _localctx = new Base_type_specContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_base_type_spec);
		try {
			setState(724);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,43,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(715);
				floating_pt_type();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(716);
				integer_type();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(717);
				char_type();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(718);
				wide_char_type();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(719);
				boolean_type();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(720);
				octet_type();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(721);
				any_type();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(722);
				object_type();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(723);
				value_base_type();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Template_type_specContext extends ParserRuleContext {
		public Sequence_typeContext sequence_type() {
			return getRuleContext(Sequence_typeContext.class,0);
		}
		public Map_typeContext map_type() {
			return getRuleContext(Map_typeContext.class,0);
		}
		public String_typeContext string_type() {
			return getRuleContext(String_typeContext.class,0);
		}
		public Wide_string_typeContext wide_string_type() {
			return getRuleContext(Wide_string_typeContext.class,0);
		}
		public Fixed_pt_typeContext fixed_pt_type() {
			return getRuleContext(Fixed_pt_typeContext.class,0);
		}
		public Template_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_template_type_spec; }
	}

	public final Template_type_specContext template_type_spec() throws RecognitionException {
		Template_type_specContext _localctx = new Template_type_specContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_template_type_spec);
		try {
			setState(731);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_SEQUENCE:
				enterOuterAlt(_localctx, 1);
				{
				setState(726);
				sequence_type();
				}
				break;
			case KW_MAP:
				enterOuterAlt(_localctx, 2);
				{
				setState(727);
				map_type();
				}
				break;
			case KW_STRING:
				enterOuterAlt(_localctx, 3);
				{
				setState(728);
				string_type();
				}
				break;
			case KW_WSTRING:
				enterOuterAlt(_localctx, 4);
				{
				setState(729);
				wide_string_type();
				}
				break;
			case KW_FIXED:
				enterOuterAlt(_localctx, 5);
				{
				setState(730);
				fixed_pt_type();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Constr_type_specContext extends ParserRuleContext {
		public Struct_typeContext struct_type() {
			return getRuleContext(Struct_typeContext.class,0);
		}
		public Union_typeContext union_type() {
			return getRuleContext(Union_typeContext.class,0);
		}
		public Enum_typeContext enum_type() {
			return getRuleContext(Enum_typeContext.class,0);
		}
		public Bitset_typeContext bitset_type() {
			return getRuleContext(Bitset_typeContext.class,0);
		}
		public Bitmask_typeContext bitmask_type() {
			return getRuleContext(Bitmask_typeContext.class,0);
		}
		public Constr_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constr_type_spec; }
	}

	public final Constr_type_specContext constr_type_spec() throws RecognitionException {
		Constr_type_specContext _localctx = new Constr_type_specContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_constr_type_spec);
		try {
			setState(738);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,45,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(733);
				struct_type();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(734);
				union_type();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(735);
				enum_type();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(736);
				bitset_type();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(737);
				bitmask_type();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Simple_declaratorsContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(IDLParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(IDLParser.ID, i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Simple_declaratorsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simple_declarators; }
	}

	public final Simple_declaratorsContext simple_declarators() throws RecognitionException {
		Simple_declaratorsContext _localctx = new Simple_declaratorsContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_simple_declarators);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(740);
			match(ID);
			setState(745);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(741);
				match(COMMA);
				setState(742);
				match(ID);
				}
				}
				setState(747);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DeclaratorsContext extends ParserRuleContext {
		public List<DeclaratorContext> declarator() {
			return getRuleContexts(DeclaratorContext.class);
		}
		public DeclaratorContext declarator(int i) {
			return getRuleContext(DeclaratorContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public DeclaratorsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declarators; }
	}

	public final DeclaratorsContext declarators() throws RecognitionException {
		DeclaratorsContext _localctx = new DeclaratorsContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_declarators);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(748);
			declarator();
			setState(753);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(749);
				match(COMMA);
				setState(750);
				declarator();
				}
				}
				setState(755);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DeclaratorContext extends ParserRuleContext {
		public Simple_declaratorContext simple_declarator() {
			return getRuleContext(Simple_declaratorContext.class,0);
		}
		public Complex_declaratorContext complex_declarator() {
			return getRuleContext(Complex_declaratorContext.class,0);
		}
		public DeclaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declarator; }
	}

	public final DeclaratorContext declarator() throws RecognitionException {
		DeclaratorContext _localctx = new DeclaratorContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_declarator);
		try {
			setState(758);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,48,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(756);
				simple_declarator();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(757);
				complex_declarator();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Simple_declaratorContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Simple_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simple_declarator; }
	}

	public final Simple_declaratorContext simple_declarator() throws RecognitionException {
		Simple_declaratorContext _localctx = new Simple_declaratorContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_simple_declarator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(760);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Complex_declaratorContext extends ParserRuleContext {
		public Array_declaratorContext array_declarator() {
			return getRuleContext(Array_declaratorContext.class,0);
		}
		public Complex_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_complex_declarator; }
	}

	public final Complex_declaratorContext complex_declarator() throws RecognitionException {
		Complex_declaratorContext _localctx = new Complex_declaratorContext(_ctx, getState());
		enterRule(_localctx, 108, RULE_complex_declarator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(762);
			array_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Floating_pt_typeContext extends ParserRuleContext {
		public TerminalNode KW_FLOAT() { return getToken(IDLParser.KW_FLOAT, 0); }
		public TerminalNode KW_DOUBLE() { return getToken(IDLParser.KW_DOUBLE, 0); }
		public TerminalNode KW_LONG() { return getToken(IDLParser.KW_LONG, 0); }
		public Floating_pt_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_floating_pt_type; }
	}

	public final Floating_pt_typeContext floating_pt_type() throws RecognitionException {
		Floating_pt_typeContext _localctx = new Floating_pt_typeContext(_ctx, getState());
		enterRule(_localctx, 110, RULE_floating_pt_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(768);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_FLOAT:
				{
				setState(764);
				match(KW_FLOAT);
				}
				break;
			case KW_DOUBLE:
				{
				setState(765);
				match(KW_DOUBLE);
				}
				break;
			case KW_LONG:
				{
				setState(766);
				match(KW_LONG);
				setState(767);
				match(KW_DOUBLE);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Integer_typeContext extends ParserRuleContext {
		public Signed_intContext signed_int() {
			return getRuleContext(Signed_intContext.class,0);
		}
		public Unsigned_intContext unsigned_int() {
			return getRuleContext(Unsigned_intContext.class,0);
		}
		public Integer_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_integer_type; }
	}

	public final Integer_typeContext integer_type() throws RecognitionException {
		Integer_typeContext _localctx = new Integer_typeContext(_ctx, getState());
		enterRule(_localctx, 112, RULE_integer_type);
		try {
			setState(772);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_SHORT:
			case KW_LONG:
			case KW_INT8:
			case KW_INT16:
			case KW_INT32:
			case KW_INT64:
				enterOuterAlt(_localctx, 1);
				{
				setState(770);
				signed_int();
				}
				break;
			case KW_UNSIGNED:
			case KW_UINT8:
			case KW_UINT16:
			case KW_UINT32:
			case KW_UINT64:
				enterOuterAlt(_localctx, 2);
				{
				setState(771);
				unsigned_int();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Signed_intContext extends ParserRuleContext {
		public Signed_short_intContext signed_short_int() {
			return getRuleContext(Signed_short_intContext.class,0);
		}
		public Signed_long_intContext signed_long_int() {
			return getRuleContext(Signed_long_intContext.class,0);
		}
		public Signed_longlong_intContext signed_longlong_int() {
			return getRuleContext(Signed_longlong_intContext.class,0);
		}
		public Signed_tiny_intContext signed_tiny_int() {
			return getRuleContext(Signed_tiny_intContext.class,0);
		}
		public Signed_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signed_int; }
	}

	public final Signed_intContext signed_int() throws RecognitionException {
		Signed_intContext _localctx = new Signed_intContext(_ctx, getState());
		enterRule(_localctx, 114, RULE_signed_int);
		try {
			setState(778);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,51,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(774);
				signed_short_int();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(775);
				signed_long_int();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(776);
				signed_longlong_int();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(777);
				signed_tiny_int();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Signed_tiny_intContext extends ParserRuleContext {
		public TerminalNode KW_INT8() { return getToken(IDLParser.KW_INT8, 0); }
		public Signed_tiny_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signed_tiny_int; }
	}

	public final Signed_tiny_intContext signed_tiny_int() throws RecognitionException {
		Signed_tiny_intContext _localctx = new Signed_tiny_intContext(_ctx, getState());
		enterRule(_localctx, 116, RULE_signed_tiny_int);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(780);
			match(KW_INT8);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Signed_short_intContext extends ParserRuleContext {
		public TerminalNode KW_SHORT() { return getToken(IDLParser.KW_SHORT, 0); }
		public TerminalNode KW_INT16() { return getToken(IDLParser.KW_INT16, 0); }
		public Signed_short_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signed_short_int; }
	}

	public final Signed_short_intContext signed_short_int() throws RecognitionException {
		Signed_short_intContext _localctx = new Signed_short_intContext(_ctx, getState());
		enterRule(_localctx, 118, RULE_signed_short_int);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(782);
			_la = _input.LA(1);
			if ( !(_la==KW_SHORT || _la==KW_INT16) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Signed_long_intContext extends ParserRuleContext {
		public TerminalNode KW_LONG() { return getToken(IDLParser.KW_LONG, 0); }
		public TerminalNode KW_INT32() { return getToken(IDLParser.KW_INT32, 0); }
		public Signed_long_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signed_long_int; }
	}

	public final Signed_long_intContext signed_long_int() throws RecognitionException {
		Signed_long_intContext _localctx = new Signed_long_intContext(_ctx, getState());
		enterRule(_localctx, 120, RULE_signed_long_int);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(784);
			_la = _input.LA(1);
			if ( !(_la==KW_LONG || _la==KW_INT32) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Signed_longlong_intContext extends ParserRuleContext {
		public List<TerminalNode> KW_LONG() { return getTokens(IDLParser.KW_LONG); }
		public TerminalNode KW_LONG(int i) {
			return getToken(IDLParser.KW_LONG, i);
		}
		public TerminalNode KW_INT64() { return getToken(IDLParser.KW_INT64, 0); }
		public Signed_longlong_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signed_longlong_int; }
	}

	public final Signed_longlong_intContext signed_longlong_int() throws RecognitionException {
		Signed_longlong_intContext _localctx = new Signed_longlong_intContext(_ctx, getState());
		enterRule(_localctx, 122, RULE_signed_longlong_int);
		try {
			setState(789);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_LONG:
				enterOuterAlt(_localctx, 1);
				{
				setState(786);
				match(KW_LONG);
				setState(787);
				match(KW_LONG);
				}
				break;
			case KW_INT64:
				enterOuterAlt(_localctx, 2);
				{
				setState(788);
				match(KW_INT64);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unsigned_intContext extends ParserRuleContext {
		public Unsigned_short_intContext unsigned_short_int() {
			return getRuleContext(Unsigned_short_intContext.class,0);
		}
		public Unsigned_long_intContext unsigned_long_int() {
			return getRuleContext(Unsigned_long_intContext.class,0);
		}
		public Unsigned_longlong_intContext unsigned_longlong_int() {
			return getRuleContext(Unsigned_longlong_intContext.class,0);
		}
		public Unsigned_tiny_intContext unsigned_tiny_int() {
			return getRuleContext(Unsigned_tiny_intContext.class,0);
		}
		public Unsigned_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unsigned_int; }
	}

	public final Unsigned_intContext unsigned_int() throws RecognitionException {
		Unsigned_intContext _localctx = new Unsigned_intContext(_ctx, getState());
		enterRule(_localctx, 124, RULE_unsigned_int);
		try {
			setState(795);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,53,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(791);
				unsigned_short_int();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(792);
				unsigned_long_int();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(793);
				unsigned_longlong_int();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(794);
				unsigned_tiny_int();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unsigned_tiny_intContext extends ParserRuleContext {
		public TerminalNode KW_UINT8() { return getToken(IDLParser.KW_UINT8, 0); }
		public Unsigned_tiny_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unsigned_tiny_int; }
	}

	public final Unsigned_tiny_intContext unsigned_tiny_int() throws RecognitionException {
		Unsigned_tiny_intContext _localctx = new Unsigned_tiny_intContext(_ctx, getState());
		enterRule(_localctx, 126, RULE_unsigned_tiny_int);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(797);
			match(KW_UINT8);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unsigned_short_intContext extends ParserRuleContext {
		public TerminalNode KW_UNSIGNED() { return getToken(IDLParser.KW_UNSIGNED, 0); }
		public TerminalNode KW_SHORT() { return getToken(IDLParser.KW_SHORT, 0); }
		public TerminalNode KW_UINT16() { return getToken(IDLParser.KW_UINT16, 0); }
		public Unsigned_short_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unsigned_short_int; }
	}

	public final Unsigned_short_intContext unsigned_short_int() throws RecognitionException {
		Unsigned_short_intContext _localctx = new Unsigned_short_intContext(_ctx, getState());
		enterRule(_localctx, 128, RULE_unsigned_short_int);
		try {
			setState(802);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_UNSIGNED:
				enterOuterAlt(_localctx, 1);
				{
				setState(799);
				match(KW_UNSIGNED);
				setState(800);
				match(KW_SHORT);
				}
				break;
			case KW_UINT16:
				enterOuterAlt(_localctx, 2);
				{
				setState(801);
				match(KW_UINT16);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unsigned_long_intContext extends ParserRuleContext {
		public TerminalNode KW_UNSIGNED() { return getToken(IDLParser.KW_UNSIGNED, 0); }
		public TerminalNode KW_LONG() { return getToken(IDLParser.KW_LONG, 0); }
		public TerminalNode KW_UINT32() { return getToken(IDLParser.KW_UINT32, 0); }
		public Unsigned_long_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unsigned_long_int; }
	}

	public final Unsigned_long_intContext unsigned_long_int() throws RecognitionException {
		Unsigned_long_intContext _localctx = new Unsigned_long_intContext(_ctx, getState());
		enterRule(_localctx, 130, RULE_unsigned_long_int);
		try {
			setState(807);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_UNSIGNED:
				enterOuterAlt(_localctx, 1);
				{
				setState(804);
				match(KW_UNSIGNED);
				setState(805);
				match(KW_LONG);
				}
				break;
			case KW_UINT32:
				enterOuterAlt(_localctx, 2);
				{
				setState(806);
				match(KW_UINT32);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Unsigned_longlong_intContext extends ParserRuleContext {
		public TerminalNode KW_UNSIGNED() { return getToken(IDLParser.KW_UNSIGNED, 0); }
		public List<TerminalNode> KW_LONG() { return getTokens(IDLParser.KW_LONG); }
		public TerminalNode KW_LONG(int i) {
			return getToken(IDLParser.KW_LONG, i);
		}
		public TerminalNode KW_UINT64() { return getToken(IDLParser.KW_UINT64, 0); }
		public Unsigned_longlong_intContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unsigned_longlong_int; }
	}

	public final Unsigned_longlong_intContext unsigned_longlong_int() throws RecognitionException {
		Unsigned_longlong_intContext _localctx = new Unsigned_longlong_intContext(_ctx, getState());
		enterRule(_localctx, 132, RULE_unsigned_longlong_int);
		try {
			setState(813);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_UNSIGNED:
				enterOuterAlt(_localctx, 1);
				{
				setState(809);
				match(KW_UNSIGNED);
				setState(810);
				match(KW_LONG);
				setState(811);
				match(KW_LONG);
				}
				break;
			case KW_UINT64:
				enterOuterAlt(_localctx, 2);
				{
				setState(812);
				match(KW_UINT64);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Char_typeContext extends ParserRuleContext {
		public TerminalNode KW_CHAR() { return getToken(IDLParser.KW_CHAR, 0); }
		public Char_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_char_type; }
	}

	public final Char_typeContext char_type() throws RecognitionException {
		Char_typeContext _localctx = new Char_typeContext(_ctx, getState());
		enterRule(_localctx, 134, RULE_char_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(815);
			match(KW_CHAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Wide_char_typeContext extends ParserRuleContext {
		public TerminalNode KW_WCHAR() { return getToken(IDLParser.KW_WCHAR, 0); }
		public Wide_char_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_wide_char_type; }
	}

	public final Wide_char_typeContext wide_char_type() throws RecognitionException {
		Wide_char_typeContext _localctx = new Wide_char_typeContext(_ctx, getState());
		enterRule(_localctx, 136, RULE_wide_char_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(817);
			match(KW_WCHAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Boolean_typeContext extends ParserRuleContext {
		public TerminalNode KW_BOOLEAN() { return getToken(IDLParser.KW_BOOLEAN, 0); }
		public Boolean_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boolean_type; }
	}

	public final Boolean_typeContext boolean_type() throws RecognitionException {
		Boolean_typeContext _localctx = new Boolean_typeContext(_ctx, getState());
		enterRule(_localctx, 138, RULE_boolean_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(819);
			match(KW_BOOLEAN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Octet_typeContext extends ParserRuleContext {
		public TerminalNode KW_OCTET() { return getToken(IDLParser.KW_OCTET, 0); }
		public Octet_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_octet_type; }
	}

	public final Octet_typeContext octet_type() throws RecognitionException {
		Octet_typeContext _localctx = new Octet_typeContext(_ctx, getState());
		enterRule(_localctx, 140, RULE_octet_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(821);
			match(KW_OCTET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Any_typeContext extends ParserRuleContext {
		public TerminalNode KW_ANY() { return getToken(IDLParser.KW_ANY, 0); }
		public Any_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_any_type; }
	}

	public final Any_typeContext any_type() throws RecognitionException {
		Any_typeContext _localctx = new Any_typeContext(_ctx, getState());
		enterRule(_localctx, 142, RULE_any_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(823);
			match(KW_ANY);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Object_typeContext extends ParserRuleContext {
		public TerminalNode KW_OBJECT() { return getToken(IDLParser.KW_OBJECT, 0); }
		public Object_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_object_type; }
	}

	public final Object_typeContext object_type() throws RecognitionException {
		Object_typeContext _localctx = new Object_typeContext(_ctx, getState());
		enterRule(_localctx, 144, RULE_object_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(825);
			match(KW_OBJECT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_declContext extends ParserRuleContext {
		public Annotation_defContext annotation_def() {
			return getRuleContext(Annotation_defContext.class,0);
		}
		public Annotation_forward_dclContext annotation_forward_dcl() {
			return getRuleContext(Annotation_forward_dclContext.class,0);
		}
		public Annotation_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_decl; }
	}

	public final Annotation_declContext annotation_decl() throws RecognitionException {
		Annotation_declContext _localctx = new Annotation_declContext(_ctx, getState());
		enterRule(_localctx, 146, RULE_annotation_decl);
		try {
			setState(829);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,57,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(827);
				annotation_def();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(828);
				annotation_forward_dcl();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_defContext extends ParserRuleContext {
		public Annotation_headerContext annotation_header() {
			return getRuleContext(Annotation_headerContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public Annotation_bodyContext annotation_body() {
			return getRuleContext(Annotation_bodyContext.class,0);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public Annotation_defContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_def; }
	}

	public final Annotation_defContext annotation_def() throws RecognitionException {
		Annotation_defContext _localctx = new Annotation_defContext(_ctx, getState());
		enterRule(_localctx, 148, RULE_annotation_def);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(831);
			annotation_header();
			setState(832);
			match(LEFT_BRACE);
			setState(833);
			annotation_body();
			setState(834);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_headerContext extends ParserRuleContext {
		public TerminalNode KW_AT_ANNOTATION() { return getToken(IDLParser.KW_AT_ANNOTATION, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Annotation_inheritance_specContext annotation_inheritance_spec() {
			return getRuleContext(Annotation_inheritance_specContext.class,0);
		}
		public Annotation_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_header; }
	}

	public final Annotation_headerContext annotation_header() throws RecognitionException {
		Annotation_headerContext _localctx = new Annotation_headerContext(_ctx, getState());
		enterRule(_localctx, 150, RULE_annotation_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(836);
			match(KW_AT_ANNOTATION);
			setState(837);
			match(ID);
			setState(839);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(838);
				annotation_inheritance_spec();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_inheritance_specContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Annotation_inheritance_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_inheritance_spec; }
	}

	public final Annotation_inheritance_specContext annotation_inheritance_spec() throws RecognitionException {
		Annotation_inheritance_specContext _localctx = new Annotation_inheritance_specContext(_ctx, getState());
		enterRule(_localctx, 152, RULE_annotation_inheritance_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(841);
			match(COLON);
			setState(842);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_bodyContext extends ParserRuleContext {
		public List<Annotation_exportContext> annotation_export() {
			return getRuleContexts(Annotation_exportContext.class);
		}
		public Annotation_exportContext annotation_export(int i) {
			return getRuleContext(Annotation_exportContext.class,i);
		}
		public Annotation_bodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_body; }
	}

	public final Annotation_bodyContext annotation_body() throws RecognitionException {
		Annotation_bodyContext _localctx = new Annotation_bodyContext(_ctx, getState());
		enterRule(_localctx, 154, RULE_annotation_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(847);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_FIXED - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(844);
				annotation_export();
				}
				}
				setState(849);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_exportContext extends ParserRuleContext {
		public Annotation_memberContext annotation_member() {
			return getRuleContext(Annotation_memberContext.class,0);
		}
		public Enum_typeContext enum_type() {
			return getRuleContext(Enum_typeContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Const_declContext const_decl() {
			return getRuleContext(Const_declContext.class,0);
		}
		public Type_defContext type_def() {
			return getRuleContext(Type_defContext.class,0);
		}
		public Annotation_exportContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_export; }
	}

	public final Annotation_exportContext annotation_export() throws RecognitionException {
		Annotation_exportContext _localctx = new Annotation_exportContext(_ctx, getState());
		enterRule(_localctx, 156, RULE_annotation_export);
		try {
			setState(860);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,60,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(850);
				annotation_member();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(851);
				enum_type();
				setState(852);
				match(SEMICOLON);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(854);
				const_decl();
				setState(855);
				match(SEMICOLON);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(857);
				type_def();
				setState(858);
				match(SEMICOLON);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_memberContext extends ParserRuleContext {
		public Annotation_member_typeContext annotation_member_type() {
			return getRuleContext(Annotation_member_typeContext.class,0);
		}
		public Simple_declaratorContext simple_declarator() {
			return getRuleContext(Simple_declaratorContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public TerminalNode KW_DEFAULT() { return getToken(IDLParser.KW_DEFAULT, 0); }
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public Annotation_memberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_member; }
	}

	public final Annotation_memberContext annotation_member() throws RecognitionException {
		Annotation_memberContext _localctx = new Annotation_memberContext(_ctx, getState());
		enterRule(_localctx, 158, RULE_annotation_member);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(862);
			annotation_member_type();
			setState(863);
			simple_declarator();
			setState(866);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_DEFAULT) {
				{
				setState(864);
				match(KW_DEFAULT);
				setState(865);
				const_exp();
				}
			}

			setState(868);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_member_typeContext extends ParserRuleContext {
		public Const_typeContext const_type() {
			return getRuleContext(Const_typeContext.class,0);
		}
		public Any_typeContext any_type() {
			return getRuleContext(Any_typeContext.class,0);
		}
		public Annotation_member_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_member_type; }
	}

	public final Annotation_member_typeContext annotation_member_type() throws RecognitionException {
		Annotation_member_typeContext _localctx = new Annotation_member_typeContext(_ctx, getState());
		enterRule(_localctx, 160, RULE_annotation_member_type);
		try {
			setState(872);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case KW_STRING:
			case KW_OCTET:
			case KW_WCHAR:
			case KW_SHORT:
			case KW_LONG:
			case KW_WSTRING:
			case KW_UNSIGNED:
			case KW_FIXED:
			case KW_CHAR:
			case KW_FLOAT:
			case KW_BOOLEAN:
			case KW_DOUBLE:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(870);
				const_type();
				}
				break;
			case KW_ANY:
				enterOuterAlt(_localctx, 2);
				{
				setState(871);
				any_type();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_forward_dclContext extends ParserRuleContext {
		public TerminalNode KW_AT_ANNOTATION() { return getToken(IDLParser.KW_AT_ANNOTATION, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Annotation_forward_dclContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_forward_dcl; }
	}

	public final Annotation_forward_dclContext annotation_forward_dcl() throws RecognitionException {
		Annotation_forward_dclContext _localctx = new Annotation_forward_dclContext(_ctx, getState());
		enterRule(_localctx, 162, RULE_annotation_forward_dcl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(874);
			match(KW_AT_ANNOTATION);
			setState(875);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bitset_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_BITSET() { return getToken(IDLParser.KW_BITSET, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public List<BitfieldContext> bitfield() {
			return getRuleContexts(BitfieldContext.class);
		}
		public BitfieldContext bitfield(int i) {
			return getRuleContext(BitfieldContext.class,i);
		}
		public Bitset_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitset_type; }
	}

	public final Bitset_typeContext bitset_type() throws RecognitionException {
		Bitset_typeContext _localctx = new Bitset_typeContext(_ctx, getState());
		enterRule(_localctx, 164, RULE_bitset_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(877);
			annapps();
			setState(878);
			match(KW_BITSET);
			setState(879);
			match(ID);
			setState(882);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(880);
				match(COLON);
				setState(881);
				scoped_name();
				}
			}

			setState(884);
			match(LEFT_BRACE);
			setState(886); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(885);
				bitfield();
				}
				}
				setState(888); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==AT || _la==KW_BITFIELD );
			setState(890);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BitfieldContext extends ParserRuleContext {
		public Bitfield_specContext bitfield_spec() {
			return getRuleContext(Bitfield_specContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Simple_declaratorsContext simple_declarators() {
			return getRuleContext(Simple_declaratorsContext.class,0);
		}
		public BitfieldContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitfield; }
	}

	public final BitfieldContext bitfield() throws RecognitionException {
		BitfieldContext _localctx = new BitfieldContext(_ctx, getState());
		enterRule(_localctx, 166, RULE_bitfield);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(892);
			bitfield_spec();
			setState(894);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ID) {
				{
				setState(893);
				simple_declarators();
				}
			}

			setState(896);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bitfield_specContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_BITFIELD() { return getToken(IDLParser.KW_BITFIELD, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public TerminalNode COMMA() { return getToken(IDLParser.COMMA, 0); }
		public Bitfield_type_specContext bitfield_type_spec() {
			return getRuleContext(Bitfield_type_specContext.class,0);
		}
		public Bitfield_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitfield_spec; }
	}

	public final Bitfield_specContext bitfield_spec() throws RecognitionException {
		Bitfield_specContext _localctx = new Bitfield_specContext(_ctx, getState());
		enterRule(_localctx, 168, RULE_bitfield_spec);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(898);
			annapps();
			setState(899);
			match(KW_BITFIELD);
			setState(900);
			match(LEFT_ANG_BRACKET);
			setState(901);
			positive_int_const();
			setState(904);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(902);
				match(COMMA);
				setState(903);
				bitfield_type_spec();
				}
			}

			setState(906);
			match(RIGHT_ANG_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bitmask_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_BITMASK() { return getToken(IDLParser.KW_BITMASK, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public List<Bit_valueContext> bit_value() {
			return getRuleContexts(Bit_valueContext.class);
		}
		public Bit_valueContext bit_value(int i) {
			return getRuleContext(Bit_valueContext.class,i);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Bitmask_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitmask_type; }
	}

	public final Bitmask_typeContext bitmask_type() throws RecognitionException {
		Bitmask_typeContext _localctx = new Bitmask_typeContext(_ctx, getState());
		enterRule(_localctx, 170, RULE_bitmask_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(908);
			annapps();
			setState(909);
			match(KW_BITMASK);
			setState(910);
			match(ID);
			setState(911);
			match(LEFT_BRACE);
			setState(912);
			bit_value();
			setState(917);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(913);
				match(COMMA);
				setState(914);
				bit_value();
				}
				}
				setState(919);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(920);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bit_valueContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Bit_valueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bit_value; }
	}

	public final Bit_valueContext bit_value() throws RecognitionException {
		Bit_valueContext _localctx = new Bit_valueContext(_ctx, getState());
		enterRule(_localctx, 172, RULE_bit_value);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(922);
			annapps();
			setState(923);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Struct_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_STRUCT() { return getToken(IDLParser.KW_STRUCT, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public Member_listContext member_list() {
			return getRuleContext(Member_listContext.class,0);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Struct_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_struct_type; }
	}

	public final Struct_typeContext struct_type() throws RecognitionException {
		Struct_typeContext _localctx = new Struct_typeContext(_ctx, getState());
		enterRule(_localctx, 174, RULE_struct_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(925);
			annapps();
			setState(926);
			match(KW_STRUCT);
			setState(927);
			match(ID);
			setState(930);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(928);
				match(COLON);
				setState(929);
				scoped_name();
				}
			}

			setState(932);
			match(LEFT_BRACE);
			setState(933);
			member_list();
			setState(934);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Member_listContext extends ParserRuleContext {
		public List<MemberContext> member() {
			return getRuleContexts(MemberContext.class);
		}
		public MemberContext member(int i) {
			return getRuleContext(MemberContext.class,i);
		}
		public Member_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_member_list; }
	}

	public final Member_listContext member_list() throws RecognitionException {
		Member_listContext _localctx = new Member_listContext(_ctx, getState());
		enterRule(_localctx, 176, RULE_member_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(939);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_OCTET) | (1L << KW_SEQUENCE) | (1L << KW_STRUCT) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_FIXED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_MAP - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(936);
				member();
				}
				}
				setState(941);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MemberContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Type_specContext type_spec() {
			return getRuleContext(Type_specContext.class,0);
		}
		public DeclaratorsContext declarators() {
			return getRuleContext(DeclaratorsContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public MemberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_member; }
	}

	public final MemberContext member() throws RecognitionException {
		MemberContext _localctx = new MemberContext(_ctx, getState());
		enterRule(_localctx, 178, RULE_member);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(942);
			annapps();
			setState(943);
			type_spec();
			setState(944);
			declarators();
			setState(945);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Union_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_UNION() { return getToken(IDLParser.KW_UNION, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_SWITCH() { return getToken(IDLParser.KW_SWITCH, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public Switch_type_specContext switch_type_spec() {
			return getRuleContext(Switch_type_specContext.class,0);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public Switch_bodyContext switch_body() {
			return getRuleContext(Switch_bodyContext.class,0);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public Union_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_union_type; }
	}

	public final Union_typeContext union_type() throws RecognitionException {
		Union_typeContext _localctx = new Union_typeContext(_ctx, getState());
		enterRule(_localctx, 180, RULE_union_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(947);
			annapps();
			setState(948);
			match(KW_UNION);
			setState(949);
			match(ID);
			setState(950);
			match(KW_SWITCH);
			setState(951);
			match(LEFT_BRACKET);
			setState(952);
			switch_type_spec();
			setState(953);
			match(RIGHT_BRACKET);
			setState(954);
			match(LEFT_BRACE);
			setState(955);
			switch_body();
			setState(956);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Switch_type_specContext extends ParserRuleContext {
		public Integer_typeContext integer_type() {
			return getRuleContext(Integer_typeContext.class,0);
		}
		public Char_typeContext char_type() {
			return getRuleContext(Char_typeContext.class,0);
		}
		public Wide_char_typeContext wide_char_type() {
			return getRuleContext(Wide_char_typeContext.class,0);
		}
		public Octet_typeContext octet_type() {
			return getRuleContext(Octet_typeContext.class,0);
		}
		public Boolean_typeContext boolean_type() {
			return getRuleContext(Boolean_typeContext.class,0);
		}
		public Enum_typeContext enum_type() {
			return getRuleContext(Enum_typeContext.class,0);
		}
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Switch_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_switch_type_spec; }
	}

	public final Switch_type_specContext switch_type_spec() throws RecognitionException {
		Switch_type_specContext _localctx = new Switch_type_specContext(_ctx, getState());
		enterRule(_localctx, 182, RULE_switch_type_spec);
		try {
			setState(965);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_SHORT:
			case KW_LONG:
			case KW_UNSIGNED:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
				enterOuterAlt(_localctx, 1);
				{
				setState(958);
				integer_type();
				}
				break;
			case KW_CHAR:
				enterOuterAlt(_localctx, 2);
				{
				setState(959);
				char_type();
				}
				break;
			case KW_WCHAR:
				enterOuterAlt(_localctx, 3);
				{
				setState(960);
				wide_char_type();
				}
				break;
			case KW_OCTET:
				enterOuterAlt(_localctx, 4);
				{
				setState(961);
				octet_type();
				}
				break;
			case KW_BOOLEAN:
				enterOuterAlt(_localctx, 5);
				{
				setState(962);
				boolean_type();
				}
				break;
			case AT:
			case KW_ENUM:
				enterOuterAlt(_localctx, 6);
				{
				setState(963);
				enum_type();
				}
				break;
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 7);
				{
				setState(964);
				scoped_name();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Switch_bodyContext extends ParserRuleContext {
		public List<Case_stmtContext> case_stmt() {
			return getRuleContexts(Case_stmtContext.class);
		}
		public Case_stmtContext case_stmt(int i) {
			return getRuleContext(Case_stmtContext.class,i);
		}
		public Switch_bodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_switch_body; }
	}

	public final Switch_bodyContext switch_body() throws RecognitionException {
		Switch_bodyContext _localctx = new Switch_bodyContext(_ctx, getState());
		enterRule(_localctx, 184, RULE_switch_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(968); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(967);
				case_stmt();
				}
				}
				setState(970); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==KW_DEFAULT || _la==KW_CASE );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Case_stmtContext extends ParserRuleContext {
		public Element_specContext element_spec() {
			return getRuleContext(Element_specContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public List<Case_labelContext> case_label() {
			return getRuleContexts(Case_labelContext.class);
		}
		public Case_labelContext case_label(int i) {
			return getRuleContext(Case_labelContext.class,i);
		}
		public Case_stmtContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_case_stmt; }
	}

	public final Case_stmtContext case_stmt() throws RecognitionException {
		Case_stmtContext _localctx = new Case_stmtContext(_ctx, getState());
		enterRule(_localctx, 186, RULE_case_stmt);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(973); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(972);
				case_label();
				}
				}
				setState(975); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==KW_DEFAULT || _la==KW_CASE );
			setState(977);
			element_spec();
			setState(978);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Case_labelContext extends ParserRuleContext {
		public TerminalNode KW_CASE() { return getToken(IDLParser.KW_CASE, 0); }
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public TerminalNode KW_DEFAULT() { return getToken(IDLParser.KW_DEFAULT, 0); }
		public Case_labelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_case_label; }
	}

	public final Case_labelContext case_label() throws RecognitionException {
		Case_labelContext _localctx = new Case_labelContext(_ctx, getState());
		enterRule(_localctx, 188, RULE_case_label);
		try {
			setState(986);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_CASE:
				enterOuterAlt(_localctx, 1);
				{
				setState(980);
				match(KW_CASE);
				setState(981);
				const_exp();
				setState(982);
				match(COLON);
				}
				break;
			case KW_DEFAULT:
				enterOuterAlt(_localctx, 2);
				{
				setState(984);
				match(KW_DEFAULT);
				setState(985);
				match(COLON);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Element_specContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Type_specContext type_spec() {
			return getRuleContext(Type_specContext.class,0);
		}
		public DeclaratorContext declarator() {
			return getRuleContext(DeclaratorContext.class,0);
		}
		public Element_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_element_spec; }
	}

	public final Element_specContext element_spec() throws RecognitionException {
		Element_specContext _localctx = new Element_specContext(_ctx, getState());
		enterRule(_localctx, 190, RULE_element_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(988);
			annapps();
			setState(989);
			type_spec();
			setState(990);
			declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Enum_typeContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_ENUM() { return getToken(IDLParser.KW_ENUM, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public List<EnumeratorContext> enumerator() {
			return getRuleContexts(EnumeratorContext.class);
		}
		public EnumeratorContext enumerator(int i) {
			return getRuleContext(EnumeratorContext.class,i);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Enum_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enum_type; }
	}

	public final Enum_typeContext enum_type() throws RecognitionException {
		Enum_typeContext _localctx = new Enum_typeContext(_ctx, getState());
		enterRule(_localctx, 192, RULE_enum_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(992);
			annapps();
			setState(993);
			match(KW_ENUM);
			setState(994);
			match(ID);
			setState(995);
			match(LEFT_BRACE);
			setState(996);
			enumerator();
			setState(1001);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(997);
				match(COMMA);
				setState(998);
				enumerator();
				}
				}
				setState(1003);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1004);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EnumeratorContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public EnumeratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumerator; }
	}

	public final EnumeratorContext enumerator() throws RecognitionException {
		EnumeratorContext _localctx = new EnumeratorContext(_ctx, getState());
		enterRule(_localctx, 194, RULE_enumerator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1006);
			annapps();
			setState(1007);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Sequence_typeContext extends ParserRuleContext {
		public TerminalNode KW_SEQUENCE() { return getToken(IDLParser.KW_SEQUENCE, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public Simple_type_specContext simple_type_spec() {
			return getRuleContext(Simple_type_specContext.class,0);
		}
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public TerminalNode COMMA() { return getToken(IDLParser.COMMA, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public Sequence_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sequence_type; }
	}

	public final Sequence_typeContext sequence_type() throws RecognitionException {
		Sequence_typeContext _localctx = new Sequence_typeContext(_ctx, getState());
		enterRule(_localctx, 196, RULE_sequence_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1009);
			match(KW_SEQUENCE);
			setState(1010);
			match(LEFT_ANG_BRACKET);
			setState(1011);
			simple_type_spec();
			setState(1014);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(1012);
				match(COMMA);
				setState(1013);
				positive_int_const();
				}
			}

			setState(1016);
			match(RIGHT_ANG_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Map_typeContext extends ParserRuleContext {
		public TerminalNode KW_MAP() { return getToken(IDLParser.KW_MAP, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public List<Simple_type_specContext> simple_type_spec() {
			return getRuleContexts(Simple_type_specContext.class);
		}
		public Simple_type_specContext simple_type_spec(int i) {
			return getRuleContext(Simple_type_specContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public Map_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_map_type; }
	}

	public final Map_typeContext map_type() throws RecognitionException {
		Map_typeContext _localctx = new Map_typeContext(_ctx, getState());
		enterRule(_localctx, 198, RULE_map_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1018);
			match(KW_MAP);
			setState(1019);
			match(LEFT_ANG_BRACKET);
			setState(1020);
			simple_type_spec();
			setState(1021);
			match(COMMA);
			setState(1022);
			simple_type_spec();
			setState(1025);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(1023);
				match(COMMA);
				setState(1024);
				positive_int_const();
				}
			}

			setState(1027);
			match(RIGHT_ANG_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class String_typeContext extends ParserRuleContext {
		public TerminalNode KW_STRING() { return getToken(IDLParser.KW_STRING, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public String_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_string_type; }
	}

	public final String_typeContext string_type() throws RecognitionException {
		String_typeContext _localctx = new String_typeContext(_ctx, getState());
		enterRule(_localctx, 200, RULE_string_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1029);
			match(KW_STRING);
			setState(1034);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LEFT_ANG_BRACKET) {
				{
				setState(1030);
				match(LEFT_ANG_BRACKET);
				setState(1031);
				positive_int_const();
				setState(1032);
				match(RIGHT_ANG_BRACKET);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Wide_string_typeContext extends ParserRuleContext {
		public TerminalNode KW_WSTRING() { return getToken(IDLParser.KW_WSTRING, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public Wide_string_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_wide_string_type; }
	}

	public final Wide_string_typeContext wide_string_type() throws RecognitionException {
		Wide_string_typeContext _localctx = new Wide_string_typeContext(_ctx, getState());
		enterRule(_localctx, 202, RULE_wide_string_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1036);
			match(KW_WSTRING);
			setState(1041);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LEFT_ANG_BRACKET) {
				{
				setState(1037);
				match(LEFT_ANG_BRACKET);
				setState(1038);
				positive_int_const();
				setState(1039);
				match(RIGHT_ANG_BRACKET);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Array_declaratorContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public List<Fixed_array_sizeContext> fixed_array_size() {
			return getRuleContexts(Fixed_array_sizeContext.class);
		}
		public Fixed_array_sizeContext fixed_array_size(int i) {
			return getRuleContext(Fixed_array_sizeContext.class,i);
		}
		public Array_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_array_declarator; }
	}

	public final Array_declaratorContext array_declarator() throws RecognitionException {
		Array_declaratorContext _localctx = new Array_declaratorContext(_ctx, getState());
		enterRule(_localctx, 204, RULE_array_declarator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1043);
			match(ID);
			setState(1045); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(1044);
				fixed_array_size();
				}
				}
				setState(1047); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==LEFT_SQUARE_BRACKET );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Fixed_array_sizeContext extends ParserRuleContext {
		public TerminalNode LEFT_SQUARE_BRACKET() { return getToken(IDLParser.LEFT_SQUARE_BRACKET, 0); }
		public Positive_int_constContext positive_int_const() {
			return getRuleContext(Positive_int_constContext.class,0);
		}
		public TerminalNode RIGHT_SQUARE_BRACKET() { return getToken(IDLParser.RIGHT_SQUARE_BRACKET, 0); }
		public Fixed_array_sizeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fixed_array_size; }
	}

	public final Fixed_array_sizeContext fixed_array_size() throws RecognitionException {
		Fixed_array_sizeContext _localctx = new Fixed_array_sizeContext(_ctx, getState());
		enterRule(_localctx, 206, RULE_fixed_array_size);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1049);
			match(LEFT_SQUARE_BRACKET);
			setState(1050);
			positive_int_const();
			setState(1051);
			match(RIGHT_SQUARE_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Attr_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Readonly_attr_specContext readonly_attr_spec() {
			return getRuleContext(Readonly_attr_specContext.class,0);
		}
		public Attr_specContext attr_spec() {
			return getRuleContext(Attr_specContext.class,0);
		}
		public Attr_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_attr_decl; }
	}

	public final Attr_declContext attr_decl() throws RecognitionException {
		Attr_declContext _localctx = new Attr_declContext(_ctx, getState());
		enterRule(_localctx, 208, RULE_attr_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1053);
			annapps();
			setState(1056);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_READONLY:
				{
				setState(1054);
				readonly_attr_spec();
				}
				break;
			case KW_ATTRIBUTE:
				{
				setState(1055);
				attr_spec();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Except_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_EXCEPTION() { return getToken(IDLParser.KW_EXCEPTION, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<MemberContext> member() {
			return getRuleContexts(MemberContext.class);
		}
		public MemberContext member(int i) {
			return getRuleContext(MemberContext.class,i);
		}
		public Except_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_except_decl; }
	}

	public final Except_declContext except_decl() throws RecognitionException {
		Except_declContext _localctx = new Except_declContext(_ctx, getState());
		enterRule(_localctx, 210, RULE_except_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1058);
			annapps();
			setState(1059);
			match(KW_EXCEPTION);
			setState(1060);
			match(ID);
			setState(1061);
			match(LEFT_BRACE);
			setState(1065);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_OCTET) | (1L << KW_SEQUENCE) | (1L << KW_STRUCT) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_FIXED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_MAP - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(1062);
				member();
				}
				}
				setState(1067);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1068);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Op_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Op_type_specContext op_type_spec() {
			return getRuleContext(Op_type_specContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Parameter_declsContext parameter_decls() {
			return getRuleContext(Parameter_declsContext.class,0);
		}
		public Op_attributeContext op_attribute() {
			return getRuleContext(Op_attributeContext.class,0);
		}
		public Raises_exprContext raises_expr() {
			return getRuleContext(Raises_exprContext.class,0);
		}
		public Context_exprContext context_expr() {
			return getRuleContext(Context_exprContext.class,0);
		}
		public Op_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_op_decl; }
	}

	public final Op_declContext op_decl() throws RecognitionException {
		Op_declContext _localctx = new Op_declContext(_ctx, getState());
		enterRule(_localctx, 212, RULE_op_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1070);
			annapps();
			setState(1072);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_ONEWAY) {
				{
				setState(1071);
				op_attribute();
				}
			}

			setState(1074);
			op_type_spec();
			setState(1075);
			match(ID);
			setState(1076);
			parameter_decls();
			setState(1078);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_RAISES) {
				{
				setState(1077);
				raises_expr();
				}
			}

			setState(1081);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_CONTEXT) {
				{
				setState(1080);
				context_expr();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Op_attributeContext extends ParserRuleContext {
		public TerminalNode KW_ONEWAY() { return getToken(IDLParser.KW_ONEWAY, 0); }
		public Op_attributeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_op_attribute; }
	}

	public final Op_attributeContext op_attribute() throws RecognitionException {
		Op_attributeContext _localctx = new Op_attributeContext(_ctx, getState());
		enterRule(_localctx, 214, RULE_op_attribute);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1083);
			match(KW_ONEWAY);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Op_type_specContext extends ParserRuleContext {
		public Param_type_specContext param_type_spec() {
			return getRuleContext(Param_type_specContext.class,0);
		}
		public TerminalNode KW_VOID() { return getToken(IDLParser.KW_VOID, 0); }
		public Op_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_op_type_spec; }
	}

	public final Op_type_specContext op_type_spec() throws RecognitionException {
		Op_type_specContext _localctx = new Op_type_specContext(_ctx, getState());
		enterRule(_localctx, 216, RULE_op_type_spec);
		try {
			setState(1087);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case KW_STRING:
			case KW_OCTET:
			case KW_WCHAR:
			case KW_SHORT:
			case KW_LONG:
			case KW_WSTRING:
			case KW_VALUEBASE:
			case KW_OBJECT:
			case KW_UNSIGNED:
			case KW_ANY:
			case KW_CHAR:
			case KW_FLOAT:
			case KW_BOOLEAN:
			case KW_DOUBLE:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(1085);
				param_type_spec();
				}
				break;
			case KW_VOID:
				enterOuterAlt(_localctx, 2);
				{
				setState(1086);
				match(KW_VOID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Parameter_declsContext extends ParserRuleContext {
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public List<Param_declContext> param_decl() {
			return getRuleContexts(Param_declContext.class);
		}
		public Param_declContext param_decl(int i) {
			return getRuleContext(Param_declContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Parameter_declsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameter_decls; }
	}

	public final Parameter_declsContext parameter_decls() throws RecognitionException {
		Parameter_declsContext _localctx = new Parameter_declsContext(_ctx, getState());
		enterRule(_localctx, 218, RULE_parameter_decls);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1089);
			match(LEFT_BRACKET);
			setState(1098);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (((((_la - 35)) & ~0x3f) == 0 && ((1L << (_la - 35)) & ((1L << (AT - 35)) | (1L << (KW_OUT - 35)) | (1L << (KW_IN - 35)) | (1L << (KW_INOUT - 35)))) != 0)) {
				{
				setState(1090);
				param_decl();
				setState(1095);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1091);
					match(COMMA);
					setState(1092);
					param_decl();
					}
					}
					setState(1097);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(1100);
			match(RIGHT_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Param_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Param_attributeContext param_attribute() {
			return getRuleContext(Param_attributeContext.class,0);
		}
		public Param_type_specContext param_type_spec() {
			return getRuleContext(Param_type_specContext.class,0);
		}
		public Simple_declaratorContext simple_declarator() {
			return getRuleContext(Simple_declaratorContext.class,0);
		}
		public Param_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_param_decl; }
	}

	public final Param_declContext param_decl() throws RecognitionException {
		Param_declContext _localctx = new Param_declContext(_ctx, getState());
		enterRule(_localctx, 220, RULE_param_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1102);
			annapps();
			setState(1103);
			param_attribute();
			setState(1104);
			param_type_spec();
			setState(1105);
			simple_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Param_attributeContext extends ParserRuleContext {
		public TerminalNode KW_IN() { return getToken(IDLParser.KW_IN, 0); }
		public TerminalNode KW_OUT() { return getToken(IDLParser.KW_OUT, 0); }
		public TerminalNode KW_INOUT() { return getToken(IDLParser.KW_INOUT, 0); }
		public Param_attributeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_param_attribute; }
	}

	public final Param_attributeContext param_attribute() throws RecognitionException {
		Param_attributeContext _localctx = new Param_attributeContext(_ctx, getState());
		enterRule(_localctx, 222, RULE_param_attribute);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1107);
			_la = _input.LA(1);
			if ( !(((((_la - 37)) & ~0x3f) == 0 && ((1L << (_la - 37)) & ((1L << (KW_OUT - 37)) | (1L << (KW_IN - 37)) | (1L << (KW_INOUT - 37)))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Raises_exprContext extends ParserRuleContext {
		public TerminalNode KW_RAISES() { return getToken(IDLParser.KW_RAISES, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public List<Scoped_nameContext> scoped_name() {
			return getRuleContexts(Scoped_nameContext.class);
		}
		public Scoped_nameContext scoped_name(int i) {
			return getRuleContext(Scoped_nameContext.class,i);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Raises_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_raises_expr; }
	}

	public final Raises_exprContext raises_expr() throws RecognitionException {
		Raises_exprContext _localctx = new Raises_exprContext(_ctx, getState());
		enterRule(_localctx, 224, RULE_raises_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1109);
			match(KW_RAISES);
			setState(1110);
			match(LEFT_BRACKET);
			setState(1111);
			scoped_name();
			setState(1116);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1112);
				match(COMMA);
				setState(1113);
				scoped_name();
				}
				}
				setState(1118);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1119);
			match(RIGHT_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Context_exprContext extends ParserRuleContext {
		public TerminalNode KW_CONTEXT() { return getToken(IDLParser.KW_CONTEXT, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public List<TerminalNode> STRING_LITERAL() { return getTokens(IDLParser.STRING_LITERAL); }
		public TerminalNode STRING_LITERAL(int i) {
			return getToken(IDLParser.STRING_LITERAL, i);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Context_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_context_expr; }
	}

	public final Context_exprContext context_expr() throws RecognitionException {
		Context_exprContext _localctx = new Context_exprContext(_ctx, getState());
		enterRule(_localctx, 226, RULE_context_expr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1121);
			match(KW_CONTEXT);
			setState(1122);
			match(LEFT_BRACKET);
			setState(1123);
			match(STRING_LITERAL);
			setState(1128);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1124);
				match(COMMA);
				setState(1125);
				match(STRING_LITERAL);
				}
				}
				setState(1130);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1131);
			match(RIGHT_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Param_type_specContext extends ParserRuleContext {
		public Base_type_specContext base_type_spec() {
			return getRuleContext(Base_type_specContext.class,0);
		}
		public String_typeContext string_type() {
			return getRuleContext(String_typeContext.class,0);
		}
		public Wide_string_typeContext wide_string_type() {
			return getRuleContext(Wide_string_typeContext.class,0);
		}
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Param_type_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_param_type_spec; }
	}

	public final Param_type_specContext param_type_spec() throws RecognitionException {
		Param_type_specContext _localctx = new Param_type_specContext(_ctx, getState());
		enterRule(_localctx, 228, RULE_param_type_spec);
		try {
			setState(1137);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_OCTET:
			case KW_WCHAR:
			case KW_SHORT:
			case KW_LONG:
			case KW_VALUEBASE:
			case KW_OBJECT:
			case KW_UNSIGNED:
			case KW_ANY:
			case KW_CHAR:
			case KW_FLOAT:
			case KW_BOOLEAN:
			case KW_DOUBLE:
			case KW_INT8:
			case KW_UINT8:
			case KW_INT16:
			case KW_UINT16:
			case KW_INT32:
			case KW_UINT32:
			case KW_INT64:
			case KW_UINT64:
				enterOuterAlt(_localctx, 1);
				{
				setState(1133);
				base_type_spec();
				}
				break;
			case KW_STRING:
				enterOuterAlt(_localctx, 2);
				{
				setState(1134);
				string_type();
				}
				break;
			case KW_WSTRING:
				enterOuterAlt(_localctx, 3);
				{
				setState(1135);
				wide_string_type();
				}
				break;
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 4);
				{
				setState(1136);
				scoped_name();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Fixed_pt_typeContext extends ParserRuleContext {
		public TerminalNode KW_FIXED() { return getToken(IDLParser.KW_FIXED, 0); }
		public TerminalNode LEFT_ANG_BRACKET() { return getToken(IDLParser.LEFT_ANG_BRACKET, 0); }
		public List<Positive_int_constContext> positive_int_const() {
			return getRuleContexts(Positive_int_constContext.class);
		}
		public Positive_int_constContext positive_int_const(int i) {
			return getRuleContext(Positive_int_constContext.class,i);
		}
		public TerminalNode COMMA() { return getToken(IDLParser.COMMA, 0); }
		public TerminalNode RIGHT_ANG_BRACKET() { return getToken(IDLParser.RIGHT_ANG_BRACKET, 0); }
		public Fixed_pt_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fixed_pt_type; }
	}

	public final Fixed_pt_typeContext fixed_pt_type() throws RecognitionException {
		Fixed_pt_typeContext _localctx = new Fixed_pt_typeContext(_ctx, getState());
		enterRule(_localctx, 230, RULE_fixed_pt_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1139);
			match(KW_FIXED);
			setState(1140);
			match(LEFT_ANG_BRACKET);
			setState(1141);
			positive_int_const();
			setState(1142);
			match(COMMA);
			setState(1143);
			positive_int_const();
			setState(1144);
			match(RIGHT_ANG_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Fixed_pt_const_typeContext extends ParserRuleContext {
		public TerminalNode KW_FIXED() { return getToken(IDLParser.KW_FIXED, 0); }
		public Fixed_pt_const_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fixed_pt_const_type; }
	}

	public final Fixed_pt_const_typeContext fixed_pt_const_type() throws RecognitionException {
		Fixed_pt_const_typeContext _localctx = new Fixed_pt_const_typeContext(_ctx, getState());
		enterRule(_localctx, 232, RULE_fixed_pt_const_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1146);
			match(KW_FIXED);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Value_base_typeContext extends ParserRuleContext {
		public TerminalNode KW_VALUEBASE() { return getToken(IDLParser.KW_VALUEBASE, 0); }
		public Value_base_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value_base_type; }
	}

	public final Value_base_typeContext value_base_type() throws RecognitionException {
		Value_base_typeContext _localctx = new Value_base_typeContext(_ctx, getState());
		enterRule(_localctx, 234, RULE_value_base_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1148);
			match(KW_VALUEBASE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Constr_forward_declContext extends ParserRuleContext {
		public TerminalNode KW_STRUCT() { return getToken(IDLParser.KW_STRUCT, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_UNION() { return getToken(IDLParser.KW_UNION, 0); }
		public Constr_forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constr_forward_decl; }
	}

	public final Constr_forward_declContext constr_forward_decl() throws RecognitionException {
		Constr_forward_declContext _localctx = new Constr_forward_declContext(_ctx, getState());
		enterRule(_localctx, 236, RULE_constr_forward_decl);
		try {
			setState(1154);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_STRUCT:
				enterOuterAlt(_localctx, 1);
				{
				setState(1150);
				match(KW_STRUCT);
				setState(1151);
				match(ID);
				}
				break;
			case KW_UNION:
				enterOuterAlt(_localctx, 2);
				{
				setState(1152);
				match(KW_UNION);
				setState(1153);
				match(ID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Import_declContext extends ParserRuleContext {
		public TerminalNode KW_IMPORT() { return getToken(IDLParser.KW_IMPORT, 0); }
		public Imported_scopeContext imported_scope() {
			return getRuleContext(Imported_scopeContext.class,0);
		}
		public Import_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_import_decl; }
	}

	public final Import_declContext import_decl() throws RecognitionException {
		Import_declContext _localctx = new Import_declContext(_ctx, getState());
		enterRule(_localctx, 238, RULE_import_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1156);
			match(KW_IMPORT);
			setState(1157);
			imported_scope();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Imported_scopeContext extends ParserRuleContext {
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public Imported_scopeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_imported_scope; }
	}

	public final Imported_scopeContext imported_scope() throws RecognitionException {
		Imported_scopeContext _localctx = new Imported_scopeContext(_ctx, getState());
		enterRule(_localctx, 240, RULE_imported_scope);
		try {
			setState(1161);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(1159);
				scoped_name();
				}
				break;
			case STRING_LITERAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(1160);
				match(STRING_LITERAL);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_id_declContext extends ParserRuleContext {
		public TerminalNode KW_TYPEID() { return getToken(IDLParser.KW_TYPEID, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public Type_id_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_id_decl; }
	}

	public final Type_id_declContext type_id_decl() throws RecognitionException {
		Type_id_declContext _localctx = new Type_id_declContext(_ctx, getState());
		enterRule(_localctx, 242, RULE_type_id_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1163);
			match(KW_TYPEID);
			setState(1164);
			scoped_name();
			setState(1165);
			match(STRING_LITERAL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Type_prefix_declContext extends ParserRuleContext {
		public TerminalNode KW_TYPEPREFIX() { return getToken(IDLParser.KW_TYPEPREFIX, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public Type_prefix_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type_prefix_decl; }
	}

	public final Type_prefix_declContext type_prefix_decl() throws RecognitionException {
		Type_prefix_declContext _localctx = new Type_prefix_declContext(_ctx, getState());
		enterRule(_localctx, 244, RULE_type_prefix_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1167);
			match(KW_TYPEPREFIX);
			setState(1168);
			scoped_name();
			setState(1169);
			match(STRING_LITERAL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Readonly_attr_specContext extends ParserRuleContext {
		public TerminalNode KW_READONLY() { return getToken(IDLParser.KW_READONLY, 0); }
		public TerminalNode KW_ATTRIBUTE() { return getToken(IDLParser.KW_ATTRIBUTE, 0); }
		public Param_type_specContext param_type_spec() {
			return getRuleContext(Param_type_specContext.class,0);
		}
		public Readonly_attr_declaratorContext readonly_attr_declarator() {
			return getRuleContext(Readonly_attr_declaratorContext.class,0);
		}
		public Readonly_attr_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_readonly_attr_spec; }
	}

	public final Readonly_attr_specContext readonly_attr_spec() throws RecognitionException {
		Readonly_attr_specContext _localctx = new Readonly_attr_specContext(_ctx, getState());
		enterRule(_localctx, 246, RULE_readonly_attr_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1171);
			match(KW_READONLY);
			setState(1172);
			match(KW_ATTRIBUTE);
			setState(1173);
			param_type_spec();
			setState(1174);
			readonly_attr_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Readonly_attr_declaratorContext extends ParserRuleContext {
		public List<Simple_declaratorContext> simple_declarator() {
			return getRuleContexts(Simple_declaratorContext.class);
		}
		public Simple_declaratorContext simple_declarator(int i) {
			return getRuleContext(Simple_declaratorContext.class,i);
		}
		public Raises_exprContext raises_expr() {
			return getRuleContext(Raises_exprContext.class,0);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Readonly_attr_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_readonly_attr_declarator; }
	}

	public final Readonly_attr_declaratorContext readonly_attr_declarator() throws RecognitionException {
		Readonly_attr_declaratorContext _localctx = new Readonly_attr_declaratorContext(_ctx, getState());
		enterRule(_localctx, 248, RULE_readonly_attr_declarator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1176);
			simple_declarator();
			setState(1185);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_RAISES:
				{
				setState(1177);
				raises_expr();
				}
				break;
			case SEMICOLON:
			case COMMA:
				{
				setState(1182);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1178);
					match(COMMA);
					setState(1179);
					simple_declarator();
					}
					}
					setState(1184);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Attr_specContext extends ParserRuleContext {
		public TerminalNode KW_ATTRIBUTE() { return getToken(IDLParser.KW_ATTRIBUTE, 0); }
		public Param_type_specContext param_type_spec() {
			return getRuleContext(Param_type_specContext.class,0);
		}
		public Attr_declaratorContext attr_declarator() {
			return getRuleContext(Attr_declaratorContext.class,0);
		}
		public Attr_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_attr_spec; }
	}

	public final Attr_specContext attr_spec() throws RecognitionException {
		Attr_specContext _localctx = new Attr_specContext(_ctx, getState());
		enterRule(_localctx, 250, RULE_attr_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1187);
			match(KW_ATTRIBUTE);
			setState(1188);
			param_type_spec();
			setState(1189);
			attr_declarator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Attr_declaratorContext extends ParserRuleContext {
		public List<Simple_declaratorContext> simple_declarator() {
			return getRuleContexts(Simple_declaratorContext.class);
		}
		public Simple_declaratorContext simple_declarator(int i) {
			return getRuleContext(Simple_declaratorContext.class,i);
		}
		public Attr_raises_exprContext attr_raises_expr() {
			return getRuleContext(Attr_raises_exprContext.class,0);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Attr_declaratorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_attr_declarator; }
	}

	public final Attr_declaratorContext attr_declarator() throws RecognitionException {
		Attr_declaratorContext _localctx = new Attr_declaratorContext(_ctx, getState());
		enterRule(_localctx, 252, RULE_attr_declarator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1191);
			simple_declarator();
			setState(1200);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_SETRAISES:
			case KW_GETRAISES:
				{
				setState(1192);
				attr_raises_expr();
				}
				break;
			case SEMICOLON:
			case COMMA:
				{
				setState(1197);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1193);
					match(COMMA);
					setState(1194);
					simple_declarator();
					}
					}
					setState(1199);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Attr_raises_exprContext extends ParserRuleContext {
		public Get_excep_exprContext get_excep_expr() {
			return getRuleContext(Get_excep_exprContext.class,0);
		}
		public Set_excep_exprContext set_excep_expr() {
			return getRuleContext(Set_excep_exprContext.class,0);
		}
		public Attr_raises_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_attr_raises_expr; }
	}

	public final Attr_raises_exprContext attr_raises_expr() throws RecognitionException {
		Attr_raises_exprContext _localctx = new Attr_raises_exprContext(_ctx, getState());
		enterRule(_localctx, 254, RULE_attr_raises_expr);
		int _la;
		try {
			setState(1207);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_GETRAISES:
				enterOuterAlt(_localctx, 1);
				{
				setState(1202);
				get_excep_expr();
				setState(1204);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==KW_SETRAISES) {
					{
					setState(1203);
					set_excep_expr();
					}
				}

				}
				break;
			case KW_SETRAISES:
				enterOuterAlt(_localctx, 2);
				{
				setState(1206);
				set_excep_expr();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Get_excep_exprContext extends ParserRuleContext {
		public TerminalNode KW_GETRAISES() { return getToken(IDLParser.KW_GETRAISES, 0); }
		public Exception_listContext exception_list() {
			return getRuleContext(Exception_listContext.class,0);
		}
		public Get_excep_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_get_excep_expr; }
	}

	public final Get_excep_exprContext get_excep_expr() throws RecognitionException {
		Get_excep_exprContext _localctx = new Get_excep_exprContext(_ctx, getState());
		enterRule(_localctx, 256, RULE_get_excep_expr);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1209);
			match(KW_GETRAISES);
			setState(1210);
			exception_list();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Set_excep_exprContext extends ParserRuleContext {
		public TerminalNode KW_SETRAISES() { return getToken(IDLParser.KW_SETRAISES, 0); }
		public Exception_listContext exception_list() {
			return getRuleContext(Exception_listContext.class,0);
		}
		public Set_excep_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_set_excep_expr; }
	}

	public final Set_excep_exprContext set_excep_expr() throws RecognitionException {
		Set_excep_exprContext _localctx = new Set_excep_exprContext(_ctx, getState());
		enterRule(_localctx, 258, RULE_set_excep_expr);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1212);
			match(KW_SETRAISES);
			setState(1213);
			exception_list();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Exception_listContext extends ParserRuleContext {
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public List<Scoped_nameContext> scoped_name() {
			return getRuleContexts(Scoped_nameContext.class);
		}
		public Scoped_nameContext scoped_name(int i) {
			return getRuleContext(Scoped_nameContext.class,i);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Exception_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_exception_list; }
	}

	public final Exception_listContext exception_list() throws RecognitionException {
		Exception_listContext _localctx = new Exception_listContext(_ctx, getState());
		enterRule(_localctx, 260, RULE_exception_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1215);
			match(LEFT_BRACKET);
			setState(1216);
			scoped_name();
			setState(1221);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1217);
				match(COMMA);
				setState(1218);
				scoped_name();
				}
				}
				setState(1223);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1224);
			match(RIGHT_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ComponentContext extends ParserRuleContext {
		public Component_declContext component_decl() {
			return getRuleContext(Component_declContext.class,0);
		}
		public Component_forward_declContext component_forward_decl() {
			return getRuleContext(Component_forward_declContext.class,0);
		}
		public ComponentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component; }
	}

	public final ComponentContext component() throws RecognitionException {
		ComponentContext _localctx = new ComponentContext(_ctx, getState());
		enterRule(_localctx, 262, RULE_component);
		try {
			setState(1228);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,100,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1226);
				component_decl();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1227);
				component_forward_decl();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_forward_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode KW_COMPONENT() { return getToken(IDLParser.KW_COMPONENT, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Component_forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_forward_decl; }
	}

	public final Component_forward_declContext component_forward_decl() throws RecognitionException {
		Component_forward_declContext _localctx = new Component_forward_declContext(_ctx, getState());
		enterRule(_localctx, 264, RULE_component_forward_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1230);
			annapps();
			setState(1231);
			match(KW_COMPONENT);
			setState(1232);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_declContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Component_headerContext component_header() {
			return getRuleContext(Component_headerContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public Component_bodyContext component_body() {
			return getRuleContext(Component_bodyContext.class,0);
		}
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public Component_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_decl; }
	}

	public final Component_declContext component_decl() throws RecognitionException {
		Component_declContext _localctx = new Component_declContext(_ctx, getState());
		enterRule(_localctx, 266, RULE_component_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1234);
			annapps();
			setState(1235);
			component_header();
			setState(1236);
			match(LEFT_BRACE);
			setState(1237);
			component_body();
			setState(1238);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_headerContext extends ParserRuleContext {
		public TerminalNode KW_COMPONENT() { return getToken(IDLParser.KW_COMPONENT, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Component_inheritance_specContext component_inheritance_spec() {
			return getRuleContext(Component_inheritance_specContext.class,0);
		}
		public Supported_interface_specContext supported_interface_spec() {
			return getRuleContext(Supported_interface_specContext.class,0);
		}
		public Component_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_header; }
	}

	public final Component_headerContext component_header() throws RecognitionException {
		Component_headerContext _localctx = new Component_headerContext(_ctx, getState());
		enterRule(_localctx, 268, RULE_component_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1240);
			match(KW_COMPONENT);
			setState(1241);
			match(ID);
			setState(1243);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(1242);
				component_inheritance_spec();
				}
			}

			setState(1246);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_SUPPORTS) {
				{
				setState(1245);
				supported_interface_spec();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Supported_interface_specContext extends ParserRuleContext {
		public TerminalNode KW_SUPPORTS() { return getToken(IDLParser.KW_SUPPORTS, 0); }
		public List<Scoped_nameContext> scoped_name() {
			return getRuleContexts(Scoped_nameContext.class);
		}
		public Scoped_nameContext scoped_name(int i) {
			return getRuleContext(Scoped_nameContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Supported_interface_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_supported_interface_spec; }
	}

	public final Supported_interface_specContext supported_interface_spec() throws RecognitionException {
		Supported_interface_specContext _localctx = new Supported_interface_specContext(_ctx, getState());
		enterRule(_localctx, 270, RULE_supported_interface_spec);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1248);
			match(KW_SUPPORTS);
			setState(1249);
			scoped_name();
			setState(1254);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1250);
				match(COMMA);
				setState(1251);
				scoped_name();
				}
				}
				setState(1256);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_inheritance_specContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Component_inheritance_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_inheritance_spec; }
	}

	public final Component_inheritance_specContext component_inheritance_spec() throws RecognitionException {
		Component_inheritance_specContext _localctx = new Component_inheritance_specContext(_ctx, getState());
		enterRule(_localctx, 272, RULE_component_inheritance_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1257);
			match(COLON);
			setState(1258);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_bodyContext extends ParserRuleContext {
		public List<Component_exportContext> component_export() {
			return getRuleContexts(Component_exportContext.class);
		}
		public Component_exportContext component_export(int i) {
			return getRuleContext(Component_exportContext.class,i);
		}
		public Component_bodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_body; }
	}

	public final Component_bodyContext component_body() throws RecognitionException {
		Component_bodyContext _localctx = new Component_bodyContext(_ctx, getState());
		enterRule(_localctx, 274, RULE_component_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1263);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (((((_la - 35)) & ~0x3f) == 0 && ((1L << (_la - 35)) & ((1L << (AT - 35)) | (1L << (KW_EMITS - 35)) | (1L << (KW_PUBLISHES - 35)) | (1L << (KW_USES - 35)) | (1L << (KW_READONLY - 35)) | (1L << (KW_PROVIDES - 35)) | (1L << (KW_CONSUMES - 35)) | (1L << (KW_ATTRIBUTE - 35)))) != 0)) {
				{
				{
				setState(1260);
				component_export();
				}
				}
				setState(1265);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Component_exportContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Provides_declContext provides_decl() {
			return getRuleContext(Provides_declContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Uses_declContext uses_decl() {
			return getRuleContext(Uses_declContext.class,0);
		}
		public Emits_declContext emits_decl() {
			return getRuleContext(Emits_declContext.class,0);
		}
		public Publishes_declContext publishes_decl() {
			return getRuleContext(Publishes_declContext.class,0);
		}
		public Consumes_declContext consumes_decl() {
			return getRuleContext(Consumes_declContext.class,0);
		}
		public Attr_declContext attr_decl() {
			return getRuleContext(Attr_declContext.class,0);
		}
		public Component_exportContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_component_export; }
	}

	public final Component_exportContext component_export() throws RecognitionException {
		Component_exportContext _localctx = new Component_exportContext(_ctx, getState());
		enterRule(_localctx, 276, RULE_component_export);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1266);
			annapps();
			setState(1285);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_PROVIDES:
				{
				setState(1267);
				provides_decl();
				setState(1268);
				match(SEMICOLON);
				}
				break;
			case KW_USES:
				{
				setState(1270);
				uses_decl();
				setState(1271);
				match(SEMICOLON);
				}
				break;
			case KW_EMITS:
				{
				setState(1273);
				emits_decl();
				setState(1274);
				match(SEMICOLON);
				}
				break;
			case KW_PUBLISHES:
				{
				setState(1276);
				publishes_decl();
				setState(1277);
				match(SEMICOLON);
				}
				break;
			case KW_CONSUMES:
				{
				setState(1279);
				consumes_decl();
				setState(1280);
				match(SEMICOLON);
				}
				break;
			case AT:
			case KW_READONLY:
			case KW_ATTRIBUTE:
				{
				setState(1282);
				attr_decl();
				setState(1283);
				match(SEMICOLON);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Provides_declContext extends ParserRuleContext {
		public TerminalNode KW_PROVIDES() { return getToken(IDLParser.KW_PROVIDES, 0); }
		public Interface_typeContext interface_type() {
			return getRuleContext(Interface_typeContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Provides_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_provides_decl; }
	}

	public final Provides_declContext provides_decl() throws RecognitionException {
		Provides_declContext _localctx = new Provides_declContext(_ctx, getState());
		enterRule(_localctx, 278, RULE_provides_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1287);
			match(KW_PROVIDES);
			setState(1288);
			interface_type();
			setState(1289);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Interface_typeContext extends ParserRuleContext {
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode KW_OBJECT() { return getToken(IDLParser.KW_OBJECT, 0); }
		public Interface_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_interface_type; }
	}

	public final Interface_typeContext interface_type() throws RecognitionException {
		Interface_typeContext _localctx = new Interface_typeContext(_ctx, getState());
		enterRule(_localctx, 280, RULE_interface_type);
		try {
			setState(1293);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DOUBLE_COLON:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(1291);
				scoped_name();
				}
				break;
			case KW_OBJECT:
				enterOuterAlt(_localctx, 2);
				{
				setState(1292);
				match(KW_OBJECT);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Uses_declContext extends ParserRuleContext {
		public TerminalNode KW_USES() { return getToken(IDLParser.KW_USES, 0); }
		public Interface_typeContext interface_type() {
			return getRuleContext(Interface_typeContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_MULTIPLE() { return getToken(IDLParser.KW_MULTIPLE, 0); }
		public Uses_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_uses_decl; }
	}

	public final Uses_declContext uses_decl() throws RecognitionException {
		Uses_declContext _localctx = new Uses_declContext(_ctx, getState());
		enterRule(_localctx, 282, RULE_uses_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1295);
			match(KW_USES);
			setState(1297);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_MULTIPLE) {
				{
				setState(1296);
				match(KW_MULTIPLE);
				}
			}

			setState(1299);
			interface_type();
			setState(1300);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Emits_declContext extends ParserRuleContext {
		public TerminalNode KW_EMITS() { return getToken(IDLParser.KW_EMITS, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Emits_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_emits_decl; }
	}

	public final Emits_declContext emits_decl() throws RecognitionException {
		Emits_declContext _localctx = new Emits_declContext(_ctx, getState());
		enterRule(_localctx, 284, RULE_emits_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1302);
			match(KW_EMITS);
			setState(1303);
			scoped_name();
			setState(1304);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Publishes_declContext extends ParserRuleContext {
		public TerminalNode KW_PUBLISHES() { return getToken(IDLParser.KW_PUBLISHES, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Publishes_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_publishes_decl; }
	}

	public final Publishes_declContext publishes_decl() throws RecognitionException {
		Publishes_declContext _localctx = new Publishes_declContext(_ctx, getState());
		enterRule(_localctx, 286, RULE_publishes_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1306);
			match(KW_PUBLISHES);
			setState(1307);
			scoped_name();
			setState(1308);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Consumes_declContext extends ParserRuleContext {
		public TerminalNode KW_CONSUMES() { return getToken(IDLParser.KW_CONSUMES, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Consumes_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_consumes_decl; }
	}

	public final Consumes_declContext consumes_decl() throws RecognitionException {
		Consumes_declContext _localctx = new Consumes_declContext(_ctx, getState());
		enterRule(_localctx, 288, RULE_consumes_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1310);
			match(KW_CONSUMES);
			setState(1311);
			scoped_name();
			setState(1312);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Home_declContext extends ParserRuleContext {
		public Home_headerContext home_header() {
			return getRuleContext(Home_headerContext.class,0);
		}
		public Home_bodyContext home_body() {
			return getRuleContext(Home_bodyContext.class,0);
		}
		public Home_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_home_decl; }
	}

	public final Home_declContext home_decl() throws RecognitionException {
		Home_declContext _localctx = new Home_declContext(_ctx, getState());
		enterRule(_localctx, 290, RULE_home_decl);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1314);
			home_header();
			setState(1315);
			home_body();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Home_headerContext extends ParserRuleContext {
		public TerminalNode KW_HOME() { return getToken(IDLParser.KW_HOME, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_MANAGES() { return getToken(IDLParser.KW_MANAGES, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Home_inheritance_specContext home_inheritance_spec() {
			return getRuleContext(Home_inheritance_specContext.class,0);
		}
		public Supported_interface_specContext supported_interface_spec() {
			return getRuleContext(Supported_interface_specContext.class,0);
		}
		public Primary_key_specContext primary_key_spec() {
			return getRuleContext(Primary_key_specContext.class,0);
		}
		public Home_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_home_header; }
	}

	public final Home_headerContext home_header() throws RecognitionException {
		Home_headerContext _localctx = new Home_headerContext(_ctx, getState());
		enterRule(_localctx, 292, RULE_home_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1317);
			match(KW_HOME);
			setState(1318);
			match(ID);
			setState(1320);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(1319);
				home_inheritance_spec();
				}
			}

			setState(1323);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_SUPPORTS) {
				{
				setState(1322);
				supported_interface_spec();
				}
			}

			setState(1325);
			match(KW_MANAGES);
			setState(1326);
			scoped_name();
			setState(1328);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_PRIMARYKEY) {
				{
				setState(1327);
				primary_key_spec();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Home_inheritance_specContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(IDLParser.COLON, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Home_inheritance_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_home_inheritance_spec; }
	}

	public final Home_inheritance_specContext home_inheritance_spec() throws RecognitionException {
		Home_inheritance_specContext _localctx = new Home_inheritance_specContext(_ctx, getState());
		enterRule(_localctx, 294, RULE_home_inheritance_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1330);
			match(COLON);
			setState(1331);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Primary_key_specContext extends ParserRuleContext {
		public TerminalNode KW_PRIMARYKEY() { return getToken(IDLParser.KW_PRIMARYKEY, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public Primary_key_specContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primary_key_spec; }
	}

	public final Primary_key_specContext primary_key_spec() throws RecognitionException {
		Primary_key_specContext _localctx = new Primary_key_specContext(_ctx, getState());
		enterRule(_localctx, 296, RULE_primary_key_spec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1333);
			match(KW_PRIMARYKEY);
			setState(1334);
			scoped_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Home_bodyContext extends ParserRuleContext {
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<Home_exportContext> home_export() {
			return getRuleContexts(Home_exportContext.class);
		}
		public Home_exportContext home_export(int i) {
			return getRuleContext(Home_exportContext.class,i);
		}
		public Home_bodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_home_body; }
	}

	public final Home_bodyContext home_body() throws RecognitionException {
		Home_bodyContext _localctx = new Home_bodyContext(_ctx, getState());
		enterRule(_localctx, 298, RULE_home_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1336);
			match(LEFT_BRACE);
			setState(1340);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_FINDER) | (1L << KW_VOID) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_FACTORY - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(1337);
				home_export();
				}
				}
				setState(1342);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1343);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Home_exportContext extends ParserRuleContext {
		public Interface_exportContext interface_export() {
			return getRuleContext(Interface_exportContext.class,0);
		}
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public Factory_declContext factory_decl() {
			return getRuleContext(Factory_declContext.class,0);
		}
		public Finder_declContext finder_decl() {
			return getRuleContext(Finder_declContext.class,0);
		}
		public Home_exportContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_home_export; }
	}

	public final Home_exportContext home_export() throws RecognitionException {
		Home_exportContext _localctx = new Home_exportContext(_ctx, getState());
		enterRule(_localctx, 300, RULE_home_export);
		try {
			setState(1353);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,113,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1345);
				interface_export();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1346);
				annapps();
				setState(1349);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case KW_FACTORY:
					{
					setState(1347);
					factory_decl();
					}
					break;
				case KW_FINDER:
					{
					setState(1348);
					finder_decl();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1351);
				match(SEMICOLON);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Factory_declContext extends ParserRuleContext {
		public TerminalNode KW_FACTORY() { return getToken(IDLParser.KW_FACTORY, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public Init_param_declsContext init_param_decls() {
			return getRuleContext(Init_param_declsContext.class,0);
		}
		public Raises_exprContext raises_expr() {
			return getRuleContext(Raises_exprContext.class,0);
		}
		public Factory_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_factory_decl; }
	}

	public final Factory_declContext factory_decl() throws RecognitionException {
		Factory_declContext _localctx = new Factory_declContext(_ctx, getState());
		enterRule(_localctx, 302, RULE_factory_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1355);
			match(KW_FACTORY);
			setState(1356);
			match(ID);
			setState(1357);
			match(LEFT_BRACKET);
			setState(1359);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AT || _la==KW_IN) {
				{
				setState(1358);
				init_param_decls();
				}
			}

			setState(1361);
			match(RIGHT_BRACKET);
			setState(1363);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_RAISES) {
				{
				setState(1362);
				raises_expr();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Finder_declContext extends ParserRuleContext {
		public TerminalNode KW_FINDER() { return getToken(IDLParser.KW_FINDER, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public Init_param_declsContext init_param_decls() {
			return getRuleContext(Init_param_declsContext.class,0);
		}
		public Raises_exprContext raises_expr() {
			return getRuleContext(Raises_exprContext.class,0);
		}
		public Finder_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_finder_decl; }
	}

	public final Finder_declContext finder_decl() throws RecognitionException {
		Finder_declContext _localctx = new Finder_declContext(_ctx, getState());
		enterRule(_localctx, 304, RULE_finder_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1365);
			match(KW_FINDER);
			setState(1366);
			match(ID);
			setState(1367);
			match(LEFT_BRACKET);
			setState(1369);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AT || _la==KW_IN) {
				{
				setState(1368);
				init_param_decls();
				}
			}

			setState(1371);
			match(RIGHT_BRACKET);
			setState(1373);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_RAISES) {
				{
				setState(1372);
				raises_expr();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EventContext extends ParserRuleContext {
		public AnnappsContext annapps() {
			return getRuleContext(AnnappsContext.class,0);
		}
		public Event_declContext event_decl() {
			return getRuleContext(Event_declContext.class,0);
		}
		public Event_abs_declContext event_abs_decl() {
			return getRuleContext(Event_abs_declContext.class,0);
		}
		public Event_forward_declContext event_forward_decl() {
			return getRuleContext(Event_forward_declContext.class,0);
		}
		public EventContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event; }
	}

	public final EventContext event() throws RecognitionException {
		EventContext _localctx = new EventContext(_ctx, getState());
		enterRule(_localctx, 306, RULE_event);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1375);
			annapps();
			setState(1379);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,118,_ctx) ) {
			case 1:
				{
				setState(1376);
				event_decl();
				}
				break;
			case 2:
				{
				setState(1377);
				event_abs_decl();
				}
				break;
			case 3:
				{
				setState(1378);
				event_forward_decl();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Event_forward_declContext extends ParserRuleContext {
		public TerminalNode KW_EVENTTYPE() { return getToken(IDLParser.KW_EVENTTYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public Event_forward_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event_forward_decl; }
	}

	public final Event_forward_declContext event_forward_decl() throws RecognitionException {
		Event_forward_declContext _localctx = new Event_forward_declContext(_ctx, getState());
		enterRule(_localctx, 308, RULE_event_forward_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1382);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_ABSTRACT) {
				{
				setState(1381);
				match(KW_ABSTRACT);
				}
			}

			setState(1384);
			match(KW_EVENTTYPE);
			setState(1385);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Event_abs_declContext extends ParserRuleContext {
		public TerminalNode KW_ABSTRACT() { return getToken(IDLParser.KW_ABSTRACT, 0); }
		public TerminalNode KW_EVENTTYPE() { return getToken(IDLParser.KW_EVENTTYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Value_inheritance_specContext value_inheritance_spec() {
			return getRuleContext(Value_inheritance_specContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<Interface_exportContext> interface_export() {
			return getRuleContexts(Interface_exportContext.class);
		}
		public Interface_exportContext interface_export(int i) {
			return getRuleContext(Interface_exportContext.class,i);
		}
		public Event_abs_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event_abs_decl; }
	}

	public final Event_abs_declContext event_abs_decl() throws RecognitionException {
		Event_abs_declContext _localctx = new Event_abs_declContext(_ctx, getState());
		enterRule(_localctx, 310, RULE_event_abs_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1387);
			match(KW_ABSTRACT);
			setState(1388);
			match(KW_EVENTTYPE);
			setState(1389);
			match(ID);
			setState(1390);
			value_inheritance_spec();
			setState(1391);
			match(LEFT_BRACE);
			setState(1395);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_VOID) | (1L << KW_WCHAR) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(1392);
				interface_export();
				}
				}
				setState(1397);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1398);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Event_declContext extends ParserRuleContext {
		public Event_headerContext event_header() {
			return getRuleContext(Event_headerContext.class,0);
		}
		public TerminalNode LEFT_BRACE() { return getToken(IDLParser.LEFT_BRACE, 0); }
		public TerminalNode RIGHT_BRACE() { return getToken(IDLParser.RIGHT_BRACE, 0); }
		public List<Value_elementContext> value_element() {
			return getRuleContexts(Value_elementContext.class);
		}
		public Value_elementContext value_element(int i) {
			return getRuleContext(Value_elementContext.class,i);
		}
		public Event_declContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event_decl; }
	}

	public final Event_declContext event_decl() throws RecognitionException {
		Event_declContext _localctx = new Event_declContext(_ctx, getState());
		enterRule(_localctx, 312, RULE_event_decl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1400);
			event_header();
			setState(1401);
			match(LEFT_BRACE);
			setState(1405);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DOUBLE_COLON) | (1L << AT) | (1L << KW_STRING) | (1L << KW_TYPEDEF) | (1L << KW_OCTET) | (1L << KW_STRUCT) | (1L << KW_NATIVE) | (1L << KW_READONLY) | (1L << KW_VOID) | (1L << KW_PRIVATE) | (1L << KW_WCHAR) | (1L << KW_PUBLIC) | (1L << KW_SHORT) | (1L << KW_LONG) | (1L << KW_ENUM))) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & ((1L << (KW_WSTRING - 64)) | (1L << (KW_FACTORY - 64)) | (1L << (KW_EXCEPTION - 64)) | (1L << (KW_CONST - 64)) | (1L << (KW_VALUEBASE - 64)) | (1L << (KW_OBJECT - 64)) | (1L << (KW_UNSIGNED - 64)) | (1L << (KW_UNION - 64)) | (1L << (KW_ONEWAY - 64)) | (1L << (KW_ANY - 64)) | (1L << (KW_CHAR - 64)) | (1L << (KW_FLOAT - 64)) | (1L << (KW_BOOLEAN - 64)) | (1L << (KW_DOUBLE - 64)) | (1L << (KW_TYPEPREFIX - 64)) | (1L << (KW_TYPEID - 64)) | (1L << (KW_ATTRIBUTE - 64)) | (1L << (KW_BITSET - 64)) | (1L << (KW_BITMASK - 64)) | (1L << (KW_INT8 - 64)) | (1L << (KW_UINT8 - 64)) | (1L << (KW_INT16 - 64)) | (1L << (KW_UINT16 - 64)) | (1L << (KW_INT32 - 64)) | (1L << (KW_UINT32 - 64)) | (1L << (KW_INT64 - 64)) | (1L << (KW_UINT64 - 64)) | (1L << (ID - 64)))) != 0)) {
				{
				{
				setState(1402);
				value_element();
				}
				}
				setState(1407);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1408);
			match(RIGHT_BRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Event_headerContext extends ParserRuleContext {
		public TerminalNode KW_EVENTTYPE() { return getToken(IDLParser.KW_EVENTTYPE, 0); }
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public Value_inheritance_specContext value_inheritance_spec() {
			return getRuleContext(Value_inheritance_specContext.class,0);
		}
		public TerminalNode KW_CUSTOM() { return getToken(IDLParser.KW_CUSTOM, 0); }
		public Event_headerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event_header; }
	}

	public final Event_headerContext event_header() throws RecognitionException {
		Event_headerContext _localctx = new Event_headerContext(_ctx, getState());
		enterRule(_localctx, 314, RULE_event_header);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1411);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==KW_CUSTOM) {
				{
				setState(1410);
				match(KW_CUSTOM);
				}
			}

			setState(1413);
			match(KW_EVENTTYPE);
			setState(1414);
			match(ID);
			setState(1415);
			value_inheritance_spec();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class AnnappsContext extends ParserRuleContext {
		public List<Annotation_applContext> annotation_appl() {
			return getRuleContexts(Annotation_applContext.class);
		}
		public Annotation_applContext annotation_appl(int i) {
			return getRuleContext(Annotation_applContext.class,i);
		}
		public AnnappsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annapps; }
	}

	public final AnnappsContext annapps() throws RecognitionException {
		AnnappsContext _localctx = new AnnappsContext(_ctx, getState());
		enterRule(_localctx, 316, RULE_annapps);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1420);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,123,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1417);
					annotation_appl();
					}
					} 
				}
				setState(1422);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,123,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_applContext extends ParserRuleContext {
		public TerminalNode AT() { return getToken(IDLParser.AT, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode LEFT_BRACKET() { return getToken(IDLParser.LEFT_BRACKET, 0); }
		public Annotation_appl_paramsContext annotation_appl_params() {
			return getRuleContext(Annotation_appl_paramsContext.class,0);
		}
		public TerminalNode RIGHT_BRACKET() { return getToken(IDLParser.RIGHT_BRACKET, 0); }
		public Annotation_applContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_appl; }
	}

	public final Annotation_applContext annotation_appl() throws RecognitionException {
		Annotation_applContext _localctx = new Annotation_applContext(_ctx, getState());
		enterRule(_localctx, 318, RULE_annotation_appl);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1423);
			match(AT);
			setState(1424);
			scoped_name();
			setState(1429);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LEFT_BRACKET) {
				{
				setState(1425);
				match(LEFT_BRACKET);
				setState(1426);
				annotation_appl_params();
				setState(1427);
				match(RIGHT_BRACKET);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_appl_paramsContext extends ParserRuleContext {
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public List<Annotation_appl_paramContext> annotation_appl_param() {
			return getRuleContexts(Annotation_appl_paramContext.class);
		}
		public Annotation_appl_paramContext annotation_appl_param(int i) {
			return getRuleContext(Annotation_appl_paramContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(IDLParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(IDLParser.COMMA, i);
		}
		public Annotation_appl_paramsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_appl_params; }
	}

	public final Annotation_appl_paramsContext annotation_appl_params() throws RecognitionException {
		Annotation_appl_paramsContext _localctx = new Annotation_appl_paramsContext(_ctx, getState());
		enterRule(_localctx, 320, RULE_annotation_appl_params);
		int _la;
		try {
			setState(1440);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,126,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1431);
				const_exp();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1432);
				annotation_appl_param();
				setState(1437);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1433);
					match(COMMA);
					setState(1434);
					annotation_appl_param();
					}
					}
					setState(1439);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Annotation_appl_paramContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(IDLParser.ID, 0); }
		public TerminalNode EQUAL() { return getToken(IDLParser.EQUAL, 0); }
		public Const_expContext const_exp() {
			return getRuleContext(Const_expContext.class,0);
		}
		public Annotation_appl_paramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_annotation_appl_param; }
	}

	public final Annotation_appl_paramContext annotation_appl_param() throws RecognitionException {
		Annotation_appl_paramContext _localctx = new Annotation_appl_paramContext(_ctx, getState());
		enterRule(_localctx, 322, RULE_annotation_appl_param);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1442);
			match(ID);
			setState(1443);
			match(EQUAL);
			setState(1444);
			const_exp();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PragmaContext extends ParserRuleContext {
		public PragmaContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pragma; }
	 
		public PragmaContext() { }
		public void copyFrom(PragmaContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class Pragma_prefixContext extends PragmaContext {
		public TerminalNode PRAGMA_PREFIX() { return getToken(IDLParser.PRAGMA_PREFIX, 0); }
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public Pragma_prefixContext(PragmaContext ctx) { copyFrom(ctx); }
	}
	public static class Pragma_versionContext extends PragmaContext {
		public TerminalNode PRAGMA_VERSION() { return getToken(IDLParser.PRAGMA_VERSION, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode VERSION_NUM() { return getToken(IDLParser.VERSION_NUM, 0); }
		public Pragma_versionContext(PragmaContext ctx) { copyFrom(ctx); }
	}
	public static class Pragma_idContext extends PragmaContext {
		public TerminalNode PRAGMA_ID() { return getToken(IDLParser.PRAGMA_ID, 0); }
		public Scoped_nameContext scoped_name() {
			return getRuleContext(Scoped_nameContext.class,0);
		}
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public Pragma_idContext(PragmaContext ctx) { copyFrom(ctx); }
	}

	public final PragmaContext pragma() throws RecognitionException {
		PragmaContext _localctx = new PragmaContext(_ctx, getState());
		enterRule(_localctx, 324, RULE_pragma);
		try {
			setState(1456);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case PRAGMA_PREFIX:
				_localctx = new Pragma_prefixContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(1446);
				match(PRAGMA_PREFIX);
				setState(1447);
				match(STRING_LITERAL);
				}
				break;
			case PRAGMA_ID:
				_localctx = new Pragma_idContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(1448);
				match(PRAGMA_ID);
				setState(1449);
				scoped_name();
				setState(1450);
				match(STRING_LITERAL);
				}
				break;
			case PRAGMA_VERSION:
				_localctx = new Pragma_versionContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(1452);
				match(PRAGMA_VERSION);
				setState(1453);
				scoped_name();
				setState(1454);
				match(VERSION_NUM);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LineContext extends ParserRuleContext {
		public TerminalNode LINE() { return getToken(IDLParser.LINE, 0); }
		public TerminalNode INTEGER_LITERAL() { return getToken(IDLParser.INTEGER_LITERAL, 0); }
		public TerminalNode STRING_LITERAL() { return getToken(IDLParser.STRING_LITERAL, 0); }
		public LineContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_line; }
	}

	public final LineContext line() throws RecognitionException {
		LineContext _localctx = new LineContext(_ctx, getState());
		enterRule(_localctx, 326, RULE_line);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1458);
			match(LINE);
			setState(1459);
			match(INTEGER_LITERAL);
			setState(1460);
			match(STRING_LITERAL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IncludeContext extends ParserRuleContext {
		public IncludeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_include; }
	 
		public IncludeContext() { }
		public void copyFrom(IncludeContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class PreprocessorImportContext extends IncludeContext {
		public Import_declContext import_decl() {
			return getRuleContext(Import_declContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(IDLParser.SEMICOLON, 0); }
		public PreprocessorImportContext(IncludeContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorIncludeContext extends IncludeContext {
		public TerminalNode SHARP() { return getToken(IDLParser.SHARP, 0); }
		public TerminalNode INCLUDE() { return getToken(IDLParser.INCLUDE, 0); }
		public Directive_textContext directive_text() {
			return getRuleContext(Directive_textContext.class,0);
		}
		public TerminalNode TEXT_NEW_LINE() { return getToken(IDLParser.TEXT_NEW_LINE, 0); }
		public PreprocessorIncludeContext(IncludeContext ctx) { copyFrom(ctx); }
	}

	public final IncludeContext include() throws RecognitionException {
		IncludeContext _localctx = new IncludeContext(_ctx, getState());
		enterRule(_localctx, 328, RULE_include);
		int _la;
		try {
			setState(1471);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SHARP:
				_localctx = new PreprocessorIncludeContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(1462);
				match(SHARP);
				setState(1463);
				match(INCLUDE);
				setState(1464);
				directive_text();
				setState(1465);
				match(TEXT_NEW_LINE);
				}
				break;
			case KW_IMPORT:
				_localctx = new PreprocessorImportContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(1467);
				import_decl();
				setState(1469);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMICOLON) {
					{
					setState(1468);
					match(SEMICOLON);
					}
				}

				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DirectiveContext extends ParserRuleContext {
		public DirectiveContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_directive; }
	 
		public DirectiveContext() { }
		public void copyFrom(DirectiveContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class PreprocessorDefContext extends DirectiveContext {
		public TerminalNode IFDEF() { return getToken(IDLParser.IFDEF, 0); }
		public TerminalNode CONDITIONAL_SYMBOL() { return getToken(IDLParser.CONDITIONAL_SYMBOL, 0); }
		public TerminalNode IFNDEF() { return getToken(IDLParser.IFNDEF, 0); }
		public TerminalNode UNDEF() { return getToken(IDLParser.UNDEF, 0); }
		public PreprocessorDefContext(DirectiveContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorConditionalContext extends DirectiveContext {
		public TerminalNode IF() { return getToken(IDLParser.IF, 0); }
		public Preprocessor_expressionContext preprocessor_expression() {
			return getRuleContext(Preprocessor_expressionContext.class,0);
		}
		public TerminalNode ELIF() { return getToken(IDLParser.ELIF, 0); }
		public TerminalNode ELSE() { return getToken(IDLParser.ELSE, 0); }
		public TerminalNode ENDIF() { return getToken(IDLParser.ENDIF, 0); }
		public Directive_textContext directive_text() {
			return getRuleContext(Directive_textContext.class,0);
		}
		public PreprocessorConditionalContext(DirectiveContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorDefineContext extends DirectiveContext {
		public TerminalNode DEFINE() { return getToken(IDLParser.DEFINE, 0); }
		public TerminalNode CONDITIONAL_SYMBOL() { return getToken(IDLParser.CONDITIONAL_SYMBOL, 0); }
		public Directive_textContext directive_text() {
			return getRuleContext(Directive_textContext.class,0);
		}
		public PreprocessorDefineContext(DirectiveContext ctx) { copyFrom(ctx); }
	}

	public final DirectiveContext directive() throws RecognitionException {
		DirectiveContext _localctx = new DirectiveContext(_ctx, getState());
		enterRule(_localctx, 330, RULE_directive);
		int _la;
		try {
			setState(1493);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case IF:
				_localctx = new PreprocessorConditionalContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(1473);
				match(IF);
				setState(1474);
				preprocessor_expression(0);
				}
				break;
			case ELIF:
				_localctx = new PreprocessorConditionalContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(1475);
				match(ELIF);
				setState(1476);
				preprocessor_expression(0);
				}
				break;
			case ELSE:
				_localctx = new PreprocessorConditionalContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(1477);
				match(ELSE);
				}
				break;
			case ENDIF:
				_localctx = new PreprocessorConditionalContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(1478);
				match(ENDIF);
				setState(1480);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==TEXT) {
					{
					setState(1479);
					directive_text();
					}
				}

				}
				break;
			case IFDEF:
				_localctx = new PreprocessorDefContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(1482);
				match(IFDEF);
				setState(1483);
				match(CONDITIONAL_SYMBOL);
				}
				break;
			case IFNDEF:
				_localctx = new PreprocessorDefContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(1484);
				match(IFNDEF);
				setState(1485);
				match(CONDITIONAL_SYMBOL);
				}
				break;
			case UNDEF:
				_localctx = new PreprocessorDefContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(1486);
				match(UNDEF);
				setState(1487);
				match(CONDITIONAL_SYMBOL);
				}
				break;
			case DEFINE:
				_localctx = new PreprocessorDefineContext(_localctx);
				enterOuterAlt(_localctx, 8);
				{
				setState(1488);
				match(DEFINE);
				setState(1489);
				match(CONDITIONAL_SYMBOL);
				setState(1491);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==TEXT) {
					{
					setState(1490);
					directive_text();
					}
				}

				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Directive_textContext extends ParserRuleContext {
		public List<TerminalNode> TEXT() { return getTokens(IDLParser.TEXT); }
		public TerminalNode TEXT(int i) {
			return getToken(IDLParser.TEXT, i);
		}
		public Directive_textContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_directive_text; }
	}

	public final Directive_textContext directive_text() throws RecognitionException {
		Directive_textContext _localctx = new Directive_textContext(_ctx, getState());
		enterRule(_localctx, 332, RULE_directive_text);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1496); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(1495);
				match(TEXT);
				}
				}
				setState(1498); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==TEXT );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Preprocessor_expressionContext extends ParserRuleContext {
		public Preprocessor_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_preprocessor_expression; }
	 
		public Preprocessor_expressionContext() { }
		public void copyFrom(Preprocessor_expressionContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class PreprocessorParenthesisContext extends Preprocessor_expressionContext {
		public TerminalNode OP_LPAREN() { return getToken(IDLParser.OP_LPAREN, 0); }
		public Preprocessor_expressionContext preprocessor_expression() {
			return getRuleContext(Preprocessor_expressionContext.class,0);
		}
		public TerminalNode OP_RPAREN() { return getToken(IDLParser.OP_RPAREN, 0); }
		public PreprocessorParenthesisContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorNotContext extends Preprocessor_expressionContext {
		public TerminalNode OP_BANG() { return getToken(IDLParser.OP_BANG, 0); }
		public Preprocessor_expressionContext preprocessor_expression() {
			return getRuleContext(Preprocessor_expressionContext.class,0);
		}
		public PreprocessorNotContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorBinaryContext extends Preprocessor_expressionContext {
		public Token op;
		public List<Preprocessor_expressionContext> preprocessor_expression() {
			return getRuleContexts(Preprocessor_expressionContext.class);
		}
		public Preprocessor_expressionContext preprocessor_expression(int i) {
			return getRuleContext(Preprocessor_expressionContext.class,i);
		}
		public TerminalNode EQUAL() { return getToken(IDLParser.EQUAL, 0); }
		public TerminalNode OP_NOTEQUAL() { return getToken(IDLParser.OP_NOTEQUAL, 0); }
		public TerminalNode OP_AND() { return getToken(IDLParser.OP_AND, 0); }
		public TerminalNode OP_OR() { return getToken(IDLParser.OP_OR, 0); }
		public TerminalNode OP_LT() { return getToken(IDLParser.OP_LT, 0); }
		public TerminalNode OP_GT() { return getToken(IDLParser.OP_GT, 0); }
		public TerminalNode OP_LE() { return getToken(IDLParser.OP_LE, 0); }
		public TerminalNode OP_GE() { return getToken(IDLParser.OP_GE, 0); }
		public PreprocessorBinaryContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorConstantContext extends Preprocessor_expressionContext {
		public TerminalNode TRUE() { return getToken(IDLParser.TRUE, 0); }
		public TerminalNode FALSE() { return getToken(IDLParser.FALSE, 0); }
		public TerminalNode DECIMAL_LITERAL() { return getToken(IDLParser.DECIMAL_LITERAL, 0); }
		public TerminalNode DIRECTIVE_STRING() { return getToken(IDLParser.DIRECTIVE_STRING, 0); }
		public PreprocessorConstantContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorConditionalSymbolContext extends Preprocessor_expressionContext {
		public TerminalNode CONDITIONAL_SYMBOL() { return getToken(IDLParser.CONDITIONAL_SYMBOL, 0); }
		public TerminalNode OP_LPAREN() { return getToken(IDLParser.OP_LPAREN, 0); }
		public Preprocessor_expressionContext preprocessor_expression() {
			return getRuleContext(Preprocessor_expressionContext.class,0);
		}
		public TerminalNode OP_RPAREN() { return getToken(IDLParser.OP_RPAREN, 0); }
		public PreprocessorConditionalSymbolContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}
	public static class PreprocessorDefinedContext extends Preprocessor_expressionContext {
		public TerminalNode DEFINED() { return getToken(IDLParser.DEFINED, 0); }
		public TerminalNode CONDITIONAL_SYMBOL() { return getToken(IDLParser.CONDITIONAL_SYMBOL, 0); }
		public TerminalNode OP_LPAREN() { return getToken(IDLParser.OP_LPAREN, 0); }
		public TerminalNode OP_RPAREN() { return getToken(IDLParser.OP_RPAREN, 0); }
		public PreprocessorDefinedContext(Preprocessor_expressionContext ctx) { copyFrom(ctx); }
	}

	public final Preprocessor_expressionContext preprocessor_expression() throws RecognitionException {
		return preprocessor_expression(0);
	}

	private Preprocessor_expressionContext preprocessor_expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		Preprocessor_expressionContext _localctx = new Preprocessor_expressionContext(_ctx, _parentState);
		Preprocessor_expressionContext _prevctx = _localctx;
		int _startState = 334;
		enterRecursionRule(_localctx, 334, RULE_preprocessor_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1525);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case TRUE:
				{
				_localctx = new PreprocessorConstantContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(1501);
				match(TRUE);
				}
				break;
			case FALSE:
				{
				_localctx = new PreprocessorConstantContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1502);
				match(FALSE);
				}
				break;
			case DECIMAL_LITERAL:
				{
				_localctx = new PreprocessorConstantContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1503);
				match(DECIMAL_LITERAL);
				}
				break;
			case DIRECTIVE_STRING:
				{
				_localctx = new PreprocessorConstantContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1504);
				match(DIRECTIVE_STRING);
				}
				break;
			case CONDITIONAL_SYMBOL:
				{
				_localctx = new PreprocessorConditionalSymbolContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1505);
				match(CONDITIONAL_SYMBOL);
				setState(1510);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,134,_ctx) ) {
				case 1:
					{
					setState(1506);
					match(OP_LPAREN);
					setState(1507);
					preprocessor_expression(0);
					setState(1508);
					match(OP_RPAREN);
					}
					break;
				}
				}
				break;
			case OP_LPAREN:
				{
				_localctx = new PreprocessorParenthesisContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1512);
				match(OP_LPAREN);
				setState(1513);
				preprocessor_expression(0);
				setState(1514);
				match(OP_RPAREN);
				}
				break;
			case OP_BANG:
				{
				_localctx = new PreprocessorNotContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1516);
				match(OP_BANG);
				setState(1517);
				preprocessor_expression(6);
				}
				break;
			case DEFINED:
				{
				_localctx = new PreprocessorDefinedContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1518);
				match(DEFINED);
				setState(1523);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case CONDITIONAL_SYMBOL:
					{
					setState(1519);
					match(CONDITIONAL_SYMBOL);
					}
					break;
				case OP_LPAREN:
					{
					setState(1520);
					match(OP_LPAREN);
					setState(1521);
					match(CONDITIONAL_SYMBOL);
					setState(1522);
					match(OP_RPAREN);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(1541);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,138,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(1539);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,137,_ctx) ) {
					case 1:
						{
						_localctx = new PreprocessorBinaryContext(new Preprocessor_expressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_preprocessor_expression);
						setState(1527);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(1528);
						((PreprocessorBinaryContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==EQUAL || _la==OP_NOTEQUAL) ) {
							((PreprocessorBinaryContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(1529);
						preprocessor_expression(6);
						}
						break;
					case 2:
						{
						_localctx = new PreprocessorBinaryContext(new Preprocessor_expressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_preprocessor_expression);
						setState(1530);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(1531);
						((PreprocessorBinaryContext)_localctx).op = match(OP_AND);
						setState(1532);
						preprocessor_expression(5);
						}
						break;
					case 3:
						{
						_localctx = new PreprocessorBinaryContext(new Preprocessor_expressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_preprocessor_expression);
						setState(1533);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(1534);
						((PreprocessorBinaryContext)_localctx).op = match(OP_OR);
						setState(1535);
						preprocessor_expression(4);
						}
						break;
					case 4:
						{
						_localctx = new PreprocessorBinaryContext(new Preprocessor_expressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_preprocessor_expression);
						setState(1536);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(1537);
						((PreprocessorBinaryContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !(((((_la - 143)) & ~0x3f) == 0 && ((1L << (_la - 143)) & ((1L << (OP_LT - 143)) | (1L << (OP_GT - 143)) | (1L << (OP_LE - 143)) | (1L << (OP_GE - 143)))) != 0)) ) {
							((PreprocessorBinaryContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(1538);
						preprocessor_expression(3);
						}
						break;
					}
					} 
				}
				setState(1543);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,138,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 167:
			return preprocessor_expression_sempred((Preprocessor_expressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean preprocessor_expression_sempred(Preprocessor_expressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 5);
		case 1:
			return precpred(_ctx, 4);
		case 2:
			return precpred(_ctx, 3);
		case 3:
			return precpred(_ctx, 2);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\u00a0\u060b\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t<\4=\t="+
		"\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\4F\tF\4G\tG\4H\tH\4I"+
		"\tI\4J\tJ\4K\tK\4L\tL\4M\tM\4N\tN\4O\tO\4P\tP\4Q\tQ\4R\tR\4S\tS\4T\tT"+
		"\4U\tU\4V\tV\4W\tW\4X\tX\4Y\tY\4Z\tZ\4[\t[\4\\\t\\\4]\t]\4^\t^\4_\t_\4"+
		"`\t`\4a\ta\4b\tb\4c\tc\4d\td\4e\te\4f\tf\4g\tg\4h\th\4i\ti\4j\tj\4k\t"+
		"k\4l\tl\4m\tm\4n\tn\4o\to\4p\tp\4q\tq\4r\tr\4s\ts\4t\tt\4u\tu\4v\tv\4"+
		"w\tw\4x\tx\4y\ty\4z\tz\4{\t{\4|\t|\4}\t}\4~\t~\4\177\t\177\4\u0080\t\u0080"+
		"\4\u0081\t\u0081\4\u0082\t\u0082\4\u0083\t\u0083\4\u0084\t\u0084\4\u0085"+
		"\t\u0085\4\u0086\t\u0086\4\u0087\t\u0087\4\u0088\t\u0088\4\u0089\t\u0089"+
		"\4\u008a\t\u008a\4\u008b\t\u008b\4\u008c\t\u008c\4\u008d\t\u008d\4\u008e"+
		"\t\u008e\4\u008f\t\u008f\4\u0090\t\u0090\4\u0091\t\u0091\4\u0092\t\u0092"+
		"\4\u0093\t\u0093\4\u0094\t\u0094\4\u0095\t\u0095\4\u0096\t\u0096\4\u0097"+
		"\t\u0097\4\u0098\t\u0098\4\u0099\t\u0099\4\u009a\t\u009a\4\u009b\t\u009b"+
		"\4\u009c\t\u009c\4\u009d\t\u009d\4\u009e\t\u009e\4\u009f\t\u009f\4\u00a0"+
		"\t\u00a0\4\u00a1\t\u00a1\4\u00a2\t\u00a2\4\u00a3\t\u00a3\4\u00a4\t\u00a4"+
		"\4\u00a5\t\u00a5\4\u00a6\t\u00a6\4\u00a7\t\u00a7\4\u00a8\t\u00a8\4\u00a9"+
		"\t\u00a9\3\2\6\2\u0154\n\2\r\2\16\2\u0155\3\3\3\3\3\3\3\3\3\3\3\3\3\3"+
		"\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\5\3\u016c\n\3\3\3"+
		"\3\3\5\3\u0170\n\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3"+
		"\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\5\3\u0187\n\3\3\4\3\4\3\4\3\4\3\4\6\4"+
		"\u018e\n\4\r\4\16\4\u018f\3\4\3\4\3\5\3\5\3\5\5\5\u0197\n\5\3\6\3\6\3"+
		"\6\3\6\3\6\3\7\5\7\u019f\n\7\3\7\3\7\3\7\3\b\5\b\u01a5\n\b\3\b\3\b\3\b"+
		"\5\b\u01aa\n\b\3\t\7\t\u01ad\n\t\f\t\16\t\u01b0\13\t\3\n\3\n\3\n\3\n\3"+
		"\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\5\n\u01c3\n\n\3\n\3"+
		"\n\5\n\u01c7\n\n\5\n\u01c9\n\n\3\13\3\13\3\13\3\13\7\13\u01cf\n\13\f\13"+
		"\16\13\u01d2\13\13\3\f\3\f\3\r\5\r\u01d7\n\r\3\r\3\r\3\r\7\r\u01dc\n\r"+
		"\f\r\16\r\u01df\13\r\3\16\3\16\3\16\3\16\3\16\5\16\u01e6\n\16\3\17\5\17"+
		"\u01e9\n\17\3\17\3\17\3\17\3\20\3\20\3\20\3\20\3\21\3\21\3\21\3\21\3\21"+
		"\3\21\7\21\u01f8\n\21\f\21\16\21\u01fb\13\21\3\21\3\21\3\22\3\22\3\22"+
		"\7\22\u0202\n\22\f\22\16\22\u0205\13\22\3\22\3\22\3\23\5\23\u020a\n\23"+
		"\3\23\3\23\3\23\3\23\3\24\3\24\5\24\u0212\n\24\3\24\3\24\3\24\7\24\u0217"+
		"\n\24\f\24\16\24\u021a\13\24\5\24\u021c\n\24\3\24\3\24\3\24\3\24\7\24"+
		"\u0222\n\24\f\24\16\24\u0225\13\24\5\24\u0227\n\24\3\25\3\25\3\26\3\26"+
		"\3\26\5\26\u022e\n\26\3\27\3\27\3\27\3\27\3\27\3\27\3\30\3\30\3\30\3\30"+
		"\3\30\5\30\u023b\n\30\3\30\3\30\5\30\u023f\n\30\3\30\3\30\3\31\3\31\3"+
		"\31\7\31\u0246\n\31\f\31\16\31\u0249\13\31\3\32\3\32\3\32\3\32\3\32\3"+
		"\33\3\33\3\34\3\34\3\34\3\34\3\34\3\34\3\34\3\35\3\35\3\35\3\35\3\35\3"+
		"\35\3\35\3\35\3\35\3\35\5\35\u0263\n\35\3\36\3\36\3\37\3\37\3\37\7\37"+
		"\u026a\n\37\f\37\16\37\u026d\13\37\3 \3 \3 \7 \u0272\n \f \16 \u0275\13"+
		" \3!\3!\3!\7!\u027a\n!\f!\16!\u027d\13!\3\"\3\"\3\"\7\"\u0282\n\"\f\""+
		"\16\"\u0285\13\"\3#\3#\3#\7#\u028a\n#\f#\16#\u028d\13#\3$\3$\3$\7$\u0292"+
		"\n$\f$\16$\u0295\13$\3%\3%\3%\3%\5%\u029b\n%\3&\3&\3\'\3\'\3\'\3\'\3\'"+
		"\3\'\5\'\u02a5\n\'\3(\3(\3)\3)\3*\3*\3*\3*\3*\3*\3*\3*\5*\u02b3\n*\3+"+
		"\3+\3+\3+\3,\3,\3,\3,\3-\3-\3-\3.\3.\5.\u02c2\n.\3/\3/\3/\5/\u02c7\n/"+
		"\3\60\3\60\3\60\5\60\u02cc\n\60\3\61\3\61\3\61\3\61\3\61\3\61\3\61\3\61"+
		"\3\61\5\61\u02d7\n\61\3\62\3\62\3\62\3\62\3\62\5\62\u02de\n\62\3\63\3"+
		"\63\3\63\3\63\3\63\5\63\u02e5\n\63\3\64\3\64\3\64\7\64\u02ea\n\64\f\64"+
		"\16\64\u02ed\13\64\3\65\3\65\3\65\7\65\u02f2\n\65\f\65\16\65\u02f5\13"+
		"\65\3\66\3\66\5\66\u02f9\n\66\3\67\3\67\38\38\39\39\39\39\59\u0303\n9"+
		"\3:\3:\5:\u0307\n:\3;\3;\3;\3;\5;\u030d\n;\3<\3<\3=\3=\3>\3>\3?\3?\3?"+
		"\5?\u0318\n?\3@\3@\3@\3@\5@\u031e\n@\3A\3A\3B\3B\3B\5B\u0325\nB\3C\3C"+
		"\3C\5C\u032a\nC\3D\3D\3D\3D\5D\u0330\nD\3E\3E\3F\3F\3G\3G\3H\3H\3I\3I"+
		"\3J\3J\3K\3K\5K\u0340\nK\3L\3L\3L\3L\3L\3M\3M\3M\5M\u034a\nM\3N\3N\3N"+
		"\3O\7O\u0350\nO\fO\16O\u0353\13O\3P\3P\3P\3P\3P\3P\3P\3P\3P\3P\5P\u035f"+
		"\nP\3Q\3Q\3Q\3Q\5Q\u0365\nQ\3Q\3Q\3R\3R\5R\u036b\nR\3S\3S\3S\3T\3T\3T"+
		"\3T\3T\5T\u0375\nT\3T\3T\6T\u0379\nT\rT\16T\u037a\3T\3T\3U\3U\5U\u0381"+
		"\nU\3U\3U\3V\3V\3V\3V\3V\3V\5V\u038b\nV\3V\3V\3W\3W\3W\3W\3W\3W\3W\7W"+
		"\u0396\nW\fW\16W\u0399\13W\3W\3W\3X\3X\3X\3Y\3Y\3Y\3Y\3Y\5Y\u03a5\nY\3"+
		"Y\3Y\3Y\3Y\3Z\7Z\u03ac\nZ\fZ\16Z\u03af\13Z\3[\3[\3[\3[\3[\3\\\3\\\3\\"+
		"\3\\\3\\\3\\\3\\\3\\\3\\\3\\\3\\\3]\3]\3]\3]\3]\3]\3]\5]\u03c8\n]\3^\6"+
		"^\u03cb\n^\r^\16^\u03cc\3_\6_\u03d0\n_\r_\16_\u03d1\3_\3_\3_\3`\3`\3`"+
		"\3`\3`\3`\5`\u03dd\n`\3a\3a\3a\3a\3b\3b\3b\3b\3b\3b\3b\7b\u03ea\nb\fb"+
		"\16b\u03ed\13b\3b\3b\3c\3c\3c\3d\3d\3d\3d\3d\5d\u03f9\nd\3d\3d\3e\3e\3"+
		"e\3e\3e\3e\3e\5e\u0404\ne\3e\3e\3f\3f\3f\3f\3f\5f\u040d\nf\3g\3g\3g\3"+
		"g\3g\5g\u0414\ng\3h\3h\6h\u0418\nh\rh\16h\u0419\3i\3i\3i\3i\3j\3j\3j\5"+
		"j\u0423\nj\3k\3k\3k\3k\3k\7k\u042a\nk\fk\16k\u042d\13k\3k\3k\3l\3l\5l"+
		"\u0433\nl\3l\3l\3l\3l\5l\u0439\nl\3l\5l\u043c\nl\3m\3m\3n\3n\5n\u0442"+
		"\nn\3o\3o\3o\3o\7o\u0448\no\fo\16o\u044b\13o\5o\u044d\no\3o\3o\3p\3p\3"+
		"p\3p\3p\3q\3q\3r\3r\3r\3r\3r\7r\u045d\nr\fr\16r\u0460\13r\3r\3r\3s\3s"+
		"\3s\3s\3s\7s\u0469\ns\fs\16s\u046c\13s\3s\3s\3t\3t\3t\3t\5t\u0474\nt\3"+
		"u\3u\3u\3u\3u\3u\3u\3v\3v\3w\3w\3x\3x\3x\3x\5x\u0485\nx\3y\3y\3y\3z\3"+
		"z\5z\u048c\nz\3{\3{\3{\3{\3|\3|\3|\3|\3}\3}\3}\3}\3}\3~\3~\3~\3~\7~\u049f"+
		"\n~\f~\16~\u04a2\13~\5~\u04a4\n~\3\177\3\177\3\177\3\177\3\u0080\3\u0080"+
		"\3\u0080\3\u0080\7\u0080\u04ae\n\u0080\f\u0080\16\u0080\u04b1\13\u0080"+
		"\5\u0080\u04b3\n\u0080\3\u0081\3\u0081\5\u0081\u04b7\n\u0081\3\u0081\5"+
		"\u0081\u04ba\n\u0081\3\u0082\3\u0082\3\u0082\3\u0083\3\u0083\3\u0083\3"+
		"\u0084\3\u0084\3\u0084\3\u0084\7\u0084\u04c6\n\u0084\f\u0084\16\u0084"+
		"\u04c9\13\u0084\3\u0084\3\u0084\3\u0085\3\u0085\5\u0085\u04cf\n\u0085"+
		"\3\u0086\3\u0086\3\u0086\3\u0086\3\u0087\3\u0087\3\u0087\3\u0087\3\u0087"+
		"\3\u0087\3\u0088\3\u0088\3\u0088\5\u0088\u04de\n\u0088\3\u0088\5\u0088"+
		"\u04e1\n\u0088\3\u0089\3\u0089\3\u0089\3\u0089\7\u0089\u04e7\n\u0089\f"+
		"\u0089\16\u0089\u04ea\13\u0089\3\u008a\3\u008a\3\u008a\3\u008b\7\u008b"+
		"\u04f0\n\u008b\f\u008b\16\u008b\u04f3\13\u008b\3\u008c\3\u008c\3\u008c"+
		"\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c"+
		"\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\3\u008c\5\u008c\u0508"+
		"\n\u008c\3\u008d\3\u008d\3\u008d\3\u008d\3\u008e\3\u008e\5\u008e\u0510"+
		"\n\u008e\3\u008f\3\u008f\5\u008f\u0514\n\u008f\3\u008f\3\u008f\3\u008f"+
		"\3\u0090\3\u0090\3\u0090\3\u0090\3\u0091\3\u0091\3\u0091\3\u0091\3\u0092"+
		"\3\u0092\3\u0092\3\u0092\3\u0093\3\u0093\3\u0093\3\u0094\3\u0094\3\u0094"+
		"\5\u0094\u052b\n\u0094\3\u0094\5\u0094\u052e\n\u0094\3\u0094\3\u0094\3"+
		"\u0094\5\u0094\u0533\n\u0094\3\u0095\3\u0095\3\u0095\3\u0096\3\u0096\3"+
		"\u0096\3\u0097\3\u0097\7\u0097\u053d\n\u0097\f\u0097\16\u0097\u0540\13"+
		"\u0097\3\u0097\3\u0097\3\u0098\3\u0098\3\u0098\3\u0098\5\u0098\u0548\n"+
		"\u0098\3\u0098\3\u0098\5\u0098\u054c\n\u0098\3\u0099\3\u0099\3\u0099\3"+
		"\u0099\5\u0099\u0552\n\u0099\3\u0099\3\u0099\5\u0099\u0556\n\u0099\3\u009a"+
		"\3\u009a\3\u009a\3\u009a\5\u009a\u055c\n\u009a\3\u009a\3\u009a\5\u009a"+
		"\u0560\n\u009a\3\u009b\3\u009b\3\u009b\3\u009b\5\u009b\u0566\n\u009b\3"+
		"\u009c\5\u009c\u0569\n\u009c\3\u009c\3\u009c\3\u009c\3\u009d\3\u009d\3"+
		"\u009d\3\u009d\3\u009d\3\u009d\7\u009d\u0574\n\u009d\f\u009d\16\u009d"+
		"\u0577\13\u009d\3\u009d\3\u009d\3\u009e\3\u009e\3\u009e\7\u009e\u057e"+
		"\n\u009e\f\u009e\16\u009e\u0581\13\u009e\3\u009e\3\u009e\3\u009f\5\u009f"+
		"\u0586\n\u009f\3\u009f\3\u009f\3\u009f\3\u009f\3\u00a0\7\u00a0\u058d\n"+
		"\u00a0\f\u00a0\16\u00a0\u0590\13\u00a0\3\u00a1\3\u00a1\3\u00a1\3\u00a1"+
		"\3\u00a1\3\u00a1\5\u00a1\u0598\n\u00a1\3\u00a2\3\u00a2\3\u00a2\3\u00a2"+
		"\7\u00a2\u059e\n\u00a2\f\u00a2\16\u00a2\u05a1\13\u00a2\5\u00a2\u05a3\n"+
		"\u00a2\3\u00a3\3\u00a3\3\u00a3\3\u00a3\3\u00a4\3\u00a4\3\u00a4\3\u00a4"+
		"\3\u00a4\3\u00a4\3\u00a4\3\u00a4\3\u00a4\3\u00a4\5\u00a4\u05b3\n\u00a4"+
		"\3\u00a5\3\u00a5\3\u00a5\3\u00a5\3\u00a6\3\u00a6\3\u00a6\3\u00a6\3\u00a6"+
		"\3\u00a6\3\u00a6\5\u00a6\u05c0\n\u00a6\5\u00a6\u05c2\n\u00a6\3\u00a7\3"+
		"\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\5\u00a7\u05cb\n\u00a7\3"+
		"\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7\3\u00a7"+
		"\5\u00a7\u05d6\n\u00a7\5\u00a7\u05d8\n\u00a7\3\u00a8\6\u00a8\u05db\n\u00a8"+
		"\r\u00a8\16\u00a8\u05dc\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9"+
		"\3\u00a9\3\u00a9\3\u00a9\3\u00a9\5\u00a9\u05e9\n\u00a9\3\u00a9\3\u00a9"+
		"\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9"+
		"\5\u00a9\u05f6\n\u00a9\5\u00a9\u05f8\n\u00a9\3\u00a9\3\u00a9\3\u00a9\3"+
		"\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9\3\u00a9"+
		"\7\u00a9\u0606\n\u00a9\f\u00a9\16\u00a9\u0609\13\u00a9\3\u00a9\2\3\u0150"+
		"\u00aa\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \"$&(*,.\60\62\64\668:<"+
		">@BDFHJLNPRTVXZ\\^`bdfhjlnprtvxz|~\u0080\u0082\u0084\u0086\u0088\u008a"+
		"\u008c\u008e\u0090\u0092\u0094\u0096\u0098\u009a\u009c\u009e\u00a0\u00a2"+
		"\u00a4\u00a6\u00a8\u00aa\u00ac\u00ae\u00b0\u00b2\u00b4\u00b6\u00b8\u00ba"+
		"\u00bc\u00be\u00c0\u00c2\u00c4\u00c6\u00c8\u00ca\u00cc\u00ce\u00d0\u00d2"+
		"\u00d4\u00d6\u00d8\u00da\u00dc\u00de\u00e0\u00e2\u00e4\u00e6\u00e8\u00ea"+
		"\u00ec\u00ee\u00f0\u00f2\u00f4\u00f6\u00f8\u00fa\u00fc\u00fe\u0100\u0102"+
		"\u0104\u0106\u0108\u010a\u010c\u010e\u0110\u0112\u0114\u0116\u0118\u011a"+
		"\u011c\u011e\u0120\u0122\u0124\u0126\u0128\u012a\u012c\u012e\u0130\u0132"+
		"\u0134\u0136\u0138\u013a\u013c\u013e\u0140\u0142\u0144\u0146\u0148\u014a"+
		"\u014c\u014e\u0150\2\16\4\2YYaa\4\299>>\3\2#$\3\2\33\34\5\2\27\27\32\32"+
		"!!\4\2\26\26\33\34\3\2\3\f\4\2??kk\4\2@@mm\5\2\'\'<<ZZ\4\2  \u008e\u008e"+
		"\3\2\u0091\u0094\2\u0643\2\u0153\3\2\2\2\4\u0186\3\2\2\2\6\u0188\3\2\2"+
		"\2\b\u0193\3\2\2\2\n\u0198\3\2\2\2\f\u019e\3\2\2\2\16\u01a4\3\2\2\2\20"+
		"\u01ae\3\2\2\2\22\u01c8\3\2\2\2\24\u01ca\3\2\2\2\26\u01d3\3\2\2\2\30\u01d6"+
		"\3\2\2\2\32\u01e0\3\2\2\2\34\u01e8\3\2\2\2\36\u01ed\3\2\2\2 \u01f1\3\2"+
		"\2\2\"\u01fe\3\2\2\2$\u0209\3\2\2\2&\u021b\3\2\2\2(\u0228\3\2\2\2*\u022d"+
		"\3\2\2\2,\u022f\3\2\2\2.\u0235\3\2\2\2\60\u0242\3\2\2\2\62\u024a\3\2\2"+
		"\2\64\u024f\3\2\2\2\66\u0251\3\2\2\28\u0262\3\2\2\2:\u0264\3\2\2\2<\u0266"+
		"\3\2\2\2>\u026e\3\2\2\2@\u0276\3\2\2\2B\u027e\3\2\2\2D\u0286\3\2\2\2F"+
		"\u028e\3\2\2\2H\u029a\3\2\2\2J\u029c\3\2\2\2L\u02a4\3\2\2\2N\u02a6\3\2"+
		"\2\2P\u02a8\3\2\2\2R\u02b2\3\2\2\2T\u02b4\3\2\2\2V\u02b8\3\2\2\2X\u02bc"+
		"\3\2\2\2Z\u02c1\3\2\2\2\\\u02c6\3\2\2\2^\u02cb\3\2\2\2`\u02d6\3\2\2\2"+
		"b\u02dd\3\2\2\2d\u02e4\3\2\2\2f\u02e6\3\2\2\2h\u02ee\3\2\2\2j\u02f8\3"+
		"\2\2\2l\u02fa\3\2\2\2n\u02fc\3\2\2\2p\u0302\3\2\2\2r\u0306\3\2\2\2t\u030c"+
		"\3\2\2\2v\u030e\3\2\2\2x\u0310\3\2\2\2z\u0312\3\2\2\2|\u0317\3\2\2\2~"+
		"\u031d\3\2\2\2\u0080\u031f\3\2\2\2\u0082\u0324\3\2\2\2\u0084\u0329\3\2"+
		"\2\2\u0086\u032f\3\2\2\2\u0088\u0331\3\2\2\2\u008a\u0333\3\2\2\2\u008c"+
		"\u0335\3\2\2\2\u008e\u0337\3\2\2\2\u0090\u0339\3\2\2\2\u0092\u033b\3\2"+
		"\2\2\u0094\u033f\3\2\2\2\u0096\u0341\3\2\2\2\u0098\u0346\3\2\2\2\u009a"+
		"\u034b\3\2\2\2\u009c\u0351\3\2\2\2\u009e\u035e\3\2\2\2\u00a0\u0360\3\2"+
		"\2\2\u00a2\u036a\3\2\2\2\u00a4\u036c\3\2\2\2\u00a6\u036f\3\2\2\2\u00a8"+
		"\u037e\3\2\2\2\u00aa\u0384\3\2\2\2\u00ac\u038e\3\2\2\2\u00ae\u039c\3\2"+
		"\2\2\u00b0\u039f\3\2\2\2\u00b2\u03ad\3\2\2\2\u00b4\u03b0\3\2\2\2\u00b6"+
		"\u03b5\3\2\2\2\u00b8\u03c7\3\2\2\2\u00ba\u03ca\3\2\2\2\u00bc\u03cf\3\2"+
		"\2\2\u00be\u03dc\3\2\2\2\u00c0\u03de\3\2\2\2\u00c2\u03e2\3\2\2\2\u00c4"+
		"\u03f0\3\2\2\2\u00c6\u03f3\3\2\2\2\u00c8\u03fc\3\2\2\2\u00ca\u0407\3\2"+
		"\2\2\u00cc\u040e\3\2\2\2\u00ce\u0415\3\2\2\2\u00d0\u041b\3\2\2\2\u00d2"+
		"\u041f\3\2\2\2\u00d4\u0424\3\2\2\2\u00d6\u0430\3\2\2\2\u00d8\u043d\3\2"+
		"\2\2\u00da\u0441\3\2\2\2\u00dc\u0443\3\2\2\2\u00de\u0450\3\2\2\2\u00e0"+
		"\u0455\3\2\2\2\u00e2\u0457\3\2\2\2\u00e4\u0463\3\2\2\2\u00e6\u0473\3\2"+
		"\2\2\u00e8\u0475\3\2\2\2\u00ea\u047c\3\2\2\2\u00ec\u047e\3\2\2\2\u00ee"+
		"\u0484\3\2\2\2\u00f0\u0486\3\2\2\2\u00f2\u048b\3\2\2\2\u00f4\u048d\3\2"+
		"\2\2\u00f6\u0491\3\2\2\2\u00f8\u0495\3\2\2\2\u00fa\u049a\3\2\2\2\u00fc"+
		"\u04a5\3\2\2\2\u00fe\u04a9\3\2\2\2\u0100\u04b9\3\2\2\2\u0102\u04bb\3\2"+
		"\2\2\u0104\u04be\3\2\2\2\u0106\u04c1\3\2\2\2\u0108\u04ce\3\2\2\2\u010a"+
		"\u04d0\3\2\2\2\u010c\u04d4\3\2\2\2\u010e\u04da\3\2\2\2\u0110\u04e2\3\2"+
		"\2\2\u0112\u04eb\3\2\2\2\u0114\u04f1\3\2\2\2\u0116\u04f4\3\2\2\2\u0118"+
		"\u0509\3\2\2\2\u011a\u050f\3\2\2\2\u011c\u0511\3\2\2\2\u011e\u0518\3\2"+
		"\2\2\u0120\u051c\3\2\2\2\u0122\u0520\3\2\2\2\u0124\u0524\3\2\2\2\u0126"+
		"\u0527\3\2\2\2\u0128\u0534\3\2\2\2\u012a\u0537\3\2\2\2\u012c\u053a\3\2"+
		"\2\2\u012e\u054b\3\2\2\2\u0130\u054d\3\2\2\2\u0132\u0557\3\2\2\2\u0134"+
		"\u0561\3\2\2\2\u0136\u0568\3\2\2\2\u0138\u056d\3\2\2\2\u013a\u057a\3\2"+
		"\2\2\u013c\u0585\3\2\2\2\u013e\u058e\3\2\2\2\u0140\u0591\3\2\2\2\u0142"+
		"\u05a2\3\2\2\2\u0144\u05a4\3\2\2\2\u0146\u05b2\3\2\2\2\u0148\u05b4\3\2"+
		"\2\2\u014a\u05c1\3\2\2\2\u014c\u05d7\3\2\2\2\u014e\u05da\3\2\2\2\u0150"+
		"\u05f7\3\2\2\2\u0152\u0154\5\4\3\2\u0153\u0152\3\2\2\2\u0154\u0155\3\2"+
		"\2\2\u0155\u0153\3\2\2\2\u0155\u0156\3\2\2\2\u0156\3\3\2\2\2\u0157\u0158"+
		"\5R*\2\u0158\u0159\7\r\2\2\u0159\u0187\3\2\2\2\u015a\u015b\5\66\34\2\u015b"+
		"\u015c\7\r\2\2\u015c\u0187\3\2\2\2\u015d\u015e\5\u00d4k\2\u015e\u015f"+
		"\7\r\2\2\u015f\u0187\3\2\2\2\u0160\u0161\5\b\5\2\u0161\u0162\7\r\2\2\u0162"+
		"\u0187\3\2\2\2\u0163\u0164\5\6\4\2\u0164\u0165\7\r\2\2\u0165\u0187\3\2"+
		"\2\2\u0166\u0167\5\32\16\2\u0167\u0168\7\r\2\2\u0168\u0187\3\2\2\2\u0169"+
		"\u016b\5\u00f4{\2\u016a\u016c\7\r\2\2\u016b\u016a\3\2\2\2\u016b\u016c"+
		"\3\2\2\2\u016c\u0187\3\2\2\2\u016d\u016f\5\u00f6|\2\u016e\u0170\7\r\2"+
		"\2\u016f\u016e\3\2\2\2\u016f\u0170\3\2\2\2\u0170\u0187\3\2\2\2\u0171\u0172"+
		"\5\u0134\u009b\2\u0172\u0173\7\r\2\2\u0173\u0187\3\2\2\2\u0174\u0175\5"+
		"\u0108\u0085\2\u0175\u0176\7\r\2\2\u0176\u0187\3\2\2\2\u0177\u0178\5\u0124"+
		"\u0093\2\u0178\u0179\7\r\2\2\u0179\u0187\3\2\2\2\u017a\u017b\5\u0094K"+
		"\2\u017b\u017c\7\r\2\2\u017c\u0187\3\2\2\2\u017d\u0187\5\u014a\u00a6\2"+
		"\u017e\u017f\7w\2\2\u017f\u0180\5\u014c\u00a7\2\u0180\u0181\7\u009a\2"+
		"\2\u0181\u0187\3\2\2\2\u0182\u0183\7w\2\2\u0183\u0187\5\u0146\u00a4\2"+
		"\u0184\u0185\7w\2\2\u0185\u0187\5\u0148\u00a5\2\u0186\u0157\3\2\2\2\u0186"+
		"\u015a\3\2\2\2\u0186\u015d\3\2\2\2\u0186\u0160\3\2\2\2\u0186\u0163\3\2"+
		"\2\2\u0186\u0166\3\2\2\2\u0186\u0169\3\2\2\2\u0186\u016d\3\2\2\2\u0186"+
		"\u0171\3\2\2\2\u0186\u0174\3\2\2\2\u0186\u0177\3\2\2\2\u0186\u017a\3\2"+
		"\2\2\u0186\u017d\3\2\2\2\u0186\u017e\3\2\2\2\u0186\u0182\3\2\2\2\u0186"+
		"\u0184\3\2\2\2\u0187\5\3\2\2\2\u0188\u0189\5\u013e\u00a0\2\u0189\u018a"+
		"\7L\2\2\u018a\u018b\7r\2\2\u018b\u018d\7\20\2\2\u018c\u018e\5\4\3\2\u018d"+
		"\u018c\3\2\2\2\u018e\u018f\3\2\2\2\u018f\u018d\3\2\2\2\u018f\u0190\3\2"+
		"\2\2\u0190\u0191\3\2\2\2\u0191\u0192\7\21\2\2\u0192\7\3\2\2\2\u0193\u0196"+
		"\5\u013e\u00a0\2\u0194\u0197\5\n\6\2\u0195\u0197\5\f\7\2\u0196\u0194\3"+
		"\2\2\2\u0196\u0195\3\2\2\2\u0197\t\3\2\2\2\u0198\u0199\5\16\b\2\u0199"+
		"\u019a\7\20\2\2\u019a\u019b\5\20\t\2\u019b\u019c\7\21\2\2\u019c\13\3\2"+
		"\2\2\u019d\u019f\t\2\2\2\u019e\u019d\3\2\2\2\u019e\u019f\3\2\2\2\u019f"+
		"\u01a0\3\2\2\2\u01a0\u01a1\7c\2\2\u01a1\u01a2\7r\2\2\u01a2\r\3\2\2\2\u01a3"+
		"\u01a5\t\2\2\2\u01a4\u01a3\3\2\2\2\u01a4\u01a5\3\2\2\2\u01a5\u01a6\3\2"+
		"\2\2\u01a6\u01a7\7c\2\2\u01a7\u01a9\7r\2\2\u01a8\u01aa\5\24\13\2\u01a9"+
		"\u01a8\3\2\2\2\u01a9\u01aa\3\2\2\2\u01aa\17\3\2\2\2\u01ab\u01ad\5\22\n"+
		"\2\u01ac\u01ab\3\2\2\2\u01ad\u01b0\3\2\2\2\u01ae\u01ac\3\2\2\2\u01ae\u01af"+
		"\3\2\2\2\u01af\21\3\2\2\2\u01b0\u01ae\3\2\2\2\u01b1\u01b2\5R*\2\u01b2"+
		"\u01b3\7\r\2\2\u01b3\u01c9\3\2\2\2\u01b4\u01b5\5\66\34\2\u01b5\u01b6\7"+
		"\r\2\2\u01b6\u01c9\3\2\2\2\u01b7\u01b8\5\u00d4k\2\u01b8\u01b9\7\r\2\2"+
		"\u01b9\u01c9\3\2\2\2\u01ba\u01bb\5\u00d2j\2\u01bb\u01bc\7\r\2\2\u01bc"+
		"\u01c9\3\2\2\2\u01bd\u01be\5\u00d6l\2\u01be\u01bf\7\r\2\2\u01bf\u01c9"+
		"\3\2\2\2\u01c0\u01c2\5\u00f4{\2\u01c1\u01c3\7\r\2\2\u01c2\u01c1\3\2\2"+
		"\2\u01c2\u01c3\3\2\2\2\u01c3\u01c9\3\2\2\2\u01c4\u01c6\5\u00f6|\2\u01c5"+
		"\u01c7\7\r\2\2\u01c6\u01c5\3\2\2\2\u01c6\u01c7\3\2\2\2\u01c7\u01c9\3\2"+
		"\2\2\u01c8\u01b1\3\2\2\2\u01c8\u01b4\3\2\2\2\u01c8\u01b7\3\2\2\2\u01c8"+
		"\u01ba\3\2\2\2\u01c8\u01bd\3\2\2\2\u01c8\u01c0\3\2\2\2\u01c8\u01c4\3\2"+
		"\2\2\u01c9\23\3\2\2\2\u01ca\u01cb\7\16\2\2\u01cb\u01d0\5\26\f\2\u01cc"+
		"\u01cd\7\17\2\2\u01cd\u01cf\5\26\f\2\u01ce\u01cc\3\2\2\2\u01cf\u01d2\3"+
		"\2\2\2\u01d0\u01ce\3\2\2\2\u01d0\u01d1\3\2\2\2\u01d1\25\3\2\2\2\u01d2"+
		"\u01d0\3\2\2\2\u01d3\u01d4\5\30\r\2\u01d4\27\3\2\2\2\u01d5\u01d7\7\"\2"+
		"\2\u01d6\u01d5\3\2\2\2\u01d6\u01d7\3\2\2\2\u01d7\u01d8\3\2\2\2\u01d8\u01dd"+
		"\7r\2\2\u01d9\u01da\7\"\2\2\u01da\u01dc\7r\2\2\u01db\u01d9\3\2\2\2\u01dc"+
		"\u01df\3\2\2\2\u01dd\u01db\3\2\2\2\u01dd\u01de\3\2\2\2\u01de\31\3\2\2"+
		"\2\u01df\u01dd\3\2\2\2\u01e0\u01e5\5\u013e\u00a0\2\u01e1\u01e6\5\"\22"+
		"\2\u01e2\u01e6\5 \21\2\u01e3\u01e6\5\36\20\2\u01e4\u01e6\5\34\17\2\u01e5"+
		"\u01e1\3\2\2\2\u01e5\u01e2\3\2\2\2\u01e5\u01e3\3\2\2\2\u01e5\u01e4\3\2"+
		"\2\2\u01e6\33\3\2\2\2\u01e7\u01e9\7Y\2\2\u01e8\u01e7\3\2\2\2\u01e8\u01e9"+
		"\3\2\2\2\u01e9\u01ea\3\2\2\2\u01ea\u01eb\7J\2\2\u01eb\u01ec\7r\2\2\u01ec"+
		"\35\3\2\2\2\u01ed\u01ee\7J\2\2\u01ee\u01ef\7r\2\2\u01ef\u01f0\5Z.\2\u01f0"+
		"\37\3\2\2\2\u01f1\u01f2\7Y\2\2\u01f2\u01f3\7J\2\2\u01f3\u01f4\7r\2\2\u01f4"+
		"\u01f5\5&\24\2\u01f5\u01f9\7\20\2\2\u01f6\u01f8\5\22\n\2\u01f7\u01f6\3"+
		"\2\2\2\u01f8\u01fb\3\2\2\2\u01f9\u01f7\3\2\2\2\u01f9\u01fa\3\2\2\2\u01fa"+
		"\u01fc\3\2\2\2\u01fb\u01f9\3\2\2\2\u01fc\u01fd\7\21\2\2\u01fd!\3\2\2\2"+
		"\u01fe\u01ff\5$\23\2\u01ff\u0203\7\20\2\2\u0200\u0202\5*\26\2\u0201\u0200"+
		"\3\2\2\2\u0202\u0205\3\2\2\2\u0203\u0201\3\2\2\2\u0203\u0204\3\2\2\2\u0204"+
		"\u0206\3\2\2\2\u0205\u0203\3\2\2\2\u0206\u0207\7\21\2\2\u0207#\3\2\2\2"+
		"\u0208\u020a\7/\2\2\u0209\u0208\3\2\2\2\u0209\u020a\3\2\2\2\u020a\u020b"+
		"\3\2\2\2\u020b\u020c\7J\2\2\u020c\u020d\7r\2\2\u020d\u020e\5&\24\2\u020e"+
		"%\3\2\2\2\u020f\u0211\7\16\2\2\u0210\u0212\7N\2\2\u0211\u0210\3\2\2\2"+
		"\u0211\u0212\3\2\2\2\u0212\u0213\3\2\2\2\u0213\u0218\5(\25\2\u0214\u0215"+
		"\7\17\2\2\u0215\u0217\5(\25\2\u0216\u0214\3\2\2\2\u0217\u021a\3\2\2\2"+
		"\u0218\u0216\3\2\2\2\u0218\u0219\3\2\2\2\u0219\u021c\3\2\2\2\u021a\u0218"+
		"\3\2\2\2\u021b\u020f\3\2\2\2\u021b\u021c\3\2\2\2\u021c\u0226\3\2\2\2\u021d"+
		"\u021e\7K\2\2\u021e\u0223\5\26\f\2\u021f\u0220\7\17\2\2\u0220\u0222\5"+
		"\26\f\2\u0221\u021f\3\2\2\2\u0222\u0225\3\2\2\2\u0223\u0221\3\2\2\2\u0223"+
		"\u0224\3\2\2\2\u0224\u0227\3\2\2\2\u0225\u0223\3\2\2\2\u0226\u021d\3\2"+
		"\2\2\u0226\u0227\3\2\2\2\u0227\'\3\2\2\2\u0228\u0229\5\30\r\2\u0229)\3"+
		"\2\2\2\u022a\u022e\5\22\n\2\u022b\u022e\5,\27\2\u022c\u022e\5.\30\2\u022d"+
		"\u022a\3\2\2\2\u022d\u022b\3\2\2\2\u022d\u022c\3\2\2\2\u022e+\3\2\2\2"+
		"\u022f\u0230\5\u013e\u00a0\2\u0230\u0231\t\3\2\2\u0231\u0232\5Z.\2\u0232"+
		"\u0233\5h\65\2\u0233\u0234\7\r\2\2\u0234-\3\2\2\2\u0235\u0236\5\u013e"+
		"\u00a0\2\u0236\u0237\7E\2\2\u0237\u0238\7r\2\2\u0238\u023a\7\22\2\2\u0239"+
		"\u023b\5\60\31\2\u023a\u0239\3\2\2\2\u023a\u023b\3\2\2\2\u023b\u023c\3"+
		"\2\2\2\u023c\u023e\7\23\2\2\u023d\u023f\5\u00e2r\2\u023e\u023d\3\2\2\2"+
		"\u023e\u023f\3\2\2\2\u023f\u0240\3\2\2\2\u0240\u0241\7\r\2\2\u0241/\3"+
		"\2\2\2\u0242\u0247\5\62\32\2\u0243\u0244\7\17\2\2\u0244\u0246\5\62\32"+
		"\2\u0245\u0243\3\2\2\2\u0246\u0249\3\2\2\2\u0247\u0245\3\2\2\2\u0247\u0248"+
		"\3\2\2\2\u0248\61\3\2\2\2\u0249\u0247\3\2\2\2\u024a\u024b\5\u013e\u00a0"+
		"\2\u024b\u024c\5\64\33\2\u024c\u024d\5\u00e6t\2\u024d\u024e\5l\67\2\u024e"+
		"\63\3\2\2\2\u024f\u0250\7<\2\2\u0250\65\3\2\2\2\u0251\u0252\5\u013e\u00a0"+
		"\2\u0252\u0253\7H\2\2\u0253\u0254\58\35\2\u0254\u0255\7r\2\2\u0255\u0256"+
		"\7 \2\2\u0256\u0257\5:\36\2\u0257\67\3\2\2\2\u0258\u0263\5r:\2\u0259\u0263"+
		"\5\u0088E\2\u025a\u0263\5\u008aF\2\u025b\u0263\5\u008cG\2\u025c\u0263"+
		"\5p9\2\u025d\u0263\5\u00caf\2\u025e\u0263\5\u00ccg\2\u025f\u0263\5\u00ea"+
		"v\2\u0260\u0263\5\30\r\2\u0261\u0263\5\u008eH\2\u0262\u0258\3\2\2\2\u0262"+
		"\u0259\3\2\2\2\u0262\u025a\3\2\2\2\u0262\u025b\3\2\2\2\u0262\u025c\3\2"+
		"\2\2\u0262\u025d\3\2\2\2\u0262\u025e\3\2\2\2\u0262\u025f\3\2\2\2\u0262"+
		"\u0260\3\2\2\2\u0262\u0261\3\2\2\2\u02639\3\2\2\2\u0264\u0265\5<\37\2"+
		"\u0265;\3\2\2\2\u0266\u026b\5> \2\u0267\u0268\7\37\2\2\u0268\u026a\5>"+
		" \2\u0269\u0267\3\2\2\2\u026a\u026d\3\2\2\2\u026b\u0269\3\2\2\2\u026b"+
		"\u026c\3\2\2\2\u026c=\3\2\2\2\u026d\u026b\3\2\2\2\u026e\u0273\5@!\2\u026f"+
		"\u0270\7\35\2\2\u0270\u0272\5@!\2\u0271\u026f\3\2\2\2\u0272\u0275\3\2"+
		"\2\2\u0273\u0271\3\2\2\2\u0273\u0274\3\2\2\2\u0274?\3\2\2\2\u0275\u0273"+
		"\3\2\2\2\u0276\u027b\5B\"\2\u0277\u0278\7\36\2\2\u0278\u027a\5B\"\2\u0279"+
		"\u0277\3\2\2\2\u027a\u027d\3\2\2\2\u027b\u0279\3\2\2\2\u027b\u027c\3\2"+
		"\2\2\u027cA\3\2\2\2\u027d\u027b\3\2\2\2\u027e\u0283\5D#\2\u027f\u0280"+
		"\t\4\2\2\u0280\u0282\5D#\2\u0281\u027f\3\2\2\2\u0282\u0285\3\2\2\2\u0283"+
		"\u0281\3\2\2\2\u0283\u0284\3\2\2\2\u0284C\3\2\2\2\u0285\u0283\3\2\2\2"+
		"\u0286\u028b\5F$\2\u0287\u0288\t\5\2\2\u0288\u028a\5F$\2\u0289\u0287\3"+
		"\2\2\2\u028a\u028d\3\2\2\2\u028b\u0289\3\2\2\2\u028b\u028c\3\2\2\2\u028c"+
		"E\3\2\2\2\u028d\u028b\3\2\2\2\u028e\u0293\5H%\2\u028f\u0290\t\6\2\2\u0290"+
		"\u0292\5H%\2\u0291\u028f\3\2\2\2\u0292\u0295\3\2\2\2\u0293\u0291\3\2\2"+
		"\2\u0293\u0294\3\2\2\2\u0294G\3\2\2\2\u0295\u0293\3\2\2\2\u0296\u0297"+
		"\5J&\2\u0297\u0298\5L\'\2\u0298\u029b\3\2\2\2\u0299\u029b\5L\'\2\u029a"+
		"\u0296\3\2\2\2\u029a\u0299\3\2\2\2\u029bI\3\2\2\2\u029c\u029d\t\7\2\2"+
		"\u029dK\3\2\2\2\u029e\u02a5\5\30\r\2\u029f\u02a5\5N(\2\u02a0\u02a1\7\22"+
		"\2\2\u02a1\u02a2\5:\36\2\u02a2\u02a3\7\23\2\2\u02a3\u02a5\3\2\2\2\u02a4"+
		"\u029e\3\2\2\2\u02a4\u029f\3\2\2\2\u02a4\u02a0\3\2\2\2\u02a5M\3\2\2\2"+
		"\u02a6\u02a7\t\b\2\2\u02a7O\3\2\2\2\u02a8\u02a9\5:\36\2\u02a9Q\3\2\2\2"+
		"\u02aa\u02b3\5T+\2\u02ab\u02b3\5\u00b0Y\2\u02ac\u02b3\5\u00b6\\\2\u02ad"+
		"\u02b3\5\u00c2b\2\u02ae\u02b3\5\u00a6T\2\u02af\u02b3\5\u00acW\2\u02b0"+
		"\u02b3\5V,\2\u02b1\u02b3\5\u00eex\2\u02b2\u02aa\3\2\2\2\u02b2\u02ab\3"+
		"\2\2\2\u02b2\u02ac\3\2\2\2\u02b2\u02ad\3\2\2\2\u02b2\u02ae\3\2\2\2\u02b2"+
		"\u02af\3\2\2\2\u02b2\u02b0\3\2\2\2\u02b2\u02b1\3\2\2\2\u02b3S\3\2\2\2"+
		"\u02b4\u02b5\5\u013e\u00a0\2\u02b5\u02b6\7,\2\2\u02b6\u02b7\5X-\2\u02b7"+
		"U\3\2\2\2\u02b8\u02b9\5\u013e\u00a0\2\u02b9\u02ba\7\64\2\2\u02ba\u02bb"+
		"\5f\64\2\u02bbW\3\2\2\2\u02bc\u02bd\5Z.\2\u02bd\u02be\5h\65\2\u02beY\3"+
		"\2\2\2\u02bf\u02c2\5\\/\2\u02c0\u02c2\5d\63\2\u02c1\u02bf\3\2\2\2\u02c1"+
		"\u02c0\3\2\2\2\u02c2[\3\2\2\2\u02c3\u02c7\5`\61\2\u02c4\u02c7\5b\62\2"+
		"\u02c5\u02c7\5\30\r\2\u02c6\u02c3\3\2\2\2\u02c6\u02c4\3\2\2\2\u02c6\u02c5"+
		"\3\2\2\2\u02c7]\3\2\2\2\u02c8\u02cc\5r:\2\u02c9\u02cc\5\u008cG\2\u02ca"+
		"\u02cc\5\u008eH\2\u02cb\u02c8\3\2\2\2\u02cb\u02c9\3\2\2\2\u02cb\u02ca"+
		"\3\2\2\2\u02cc_\3\2\2\2\u02cd\u02d7\5p9\2\u02ce\u02d7\5r:\2\u02cf\u02d7"+
		"\5\u0088E\2\u02d0\u02d7\5\u008aF\2\u02d1\u02d7\5\u008cG\2\u02d2\u02d7"+
		"\5\u008eH\2\u02d3\u02d7\5\u0090I\2\u02d4\u02d7\5\u0092J\2\u02d5\u02d7"+
		"\5\u00ecw\2\u02d6\u02cd\3\2\2\2\u02d6\u02ce\3\2\2\2\u02d6\u02cf\3\2\2"+
		"\2\u02d6\u02d0\3\2\2\2\u02d6\u02d1\3\2\2\2\u02d6\u02d2\3\2\2\2\u02d6\u02d3"+
		"\3\2\2\2\u02d6\u02d4\3\2\2\2\u02d6\u02d5\3\2\2\2\u02d7a\3\2\2\2\u02d8"+
		"\u02de\5\u00c6d\2\u02d9\u02de\5\u00c8e\2\u02da\u02de\5\u00caf\2\u02db"+
		"\u02de\5\u00ccg\2\u02dc\u02de\5\u00e8u\2\u02dd\u02d8\3\2\2\2\u02dd\u02d9"+
		"\3\2\2\2\u02dd\u02da\3\2\2\2\u02dd\u02db\3\2\2\2\u02dd\u02dc\3\2\2\2\u02de"+
		"c\3\2\2\2\u02df\u02e5\5\u00b0Y\2\u02e0\u02e5\5\u00b6\\\2\u02e1\u02e5\5"+
		"\u00c2b\2\u02e2\u02e5\5\u00a6T\2\u02e3\u02e5\5\u00acW\2\u02e4\u02df\3"+
		"\2\2\2\u02e4\u02e0\3\2\2\2\u02e4\u02e1\3\2\2\2\u02e4\u02e2\3\2\2\2\u02e4"+
		"\u02e3\3\2\2\2\u02e5e\3\2\2\2\u02e6\u02eb\7r\2\2\u02e7\u02e8\7\17\2\2"+
		"\u02e8\u02ea\7r\2\2\u02e9\u02e7\3\2\2\2\u02ea\u02ed\3\2\2\2\u02eb\u02e9"+
		"\3\2\2\2\u02eb\u02ec\3\2\2\2\u02ecg\3\2\2\2\u02ed\u02eb\3\2\2\2\u02ee"+
		"\u02f3\5j\66\2\u02ef\u02f0\7\17\2\2\u02f0\u02f2\5j\66\2\u02f1\u02ef\3"+
		"\2\2\2\u02f2\u02f5\3\2\2\2\u02f3\u02f1\3\2\2\2\u02f3\u02f4\3\2\2\2\u02f4"+
		"i\3\2\2\2\u02f5\u02f3\3\2\2\2\u02f6\u02f9\5l\67\2\u02f7\u02f9\5n8\2\u02f8"+
		"\u02f6\3\2\2\2\u02f8\u02f7\3\2\2\2\u02f9k\3\2\2\2\u02fa\u02fb\7r\2\2\u02fb"+
		"m\3\2\2\2\u02fc\u02fd\5\u00ceh\2\u02fdo\3\2\2\2\u02fe\u0303\7V\2\2\u02ff"+
		"\u0303\7]\2\2\u0300\u0301\7@\2\2\u0301\u0303\7]\2\2\u0302\u02fe\3\2\2"+
		"\2\u0302\u02ff\3\2\2\2\u0302\u0300\3\2\2\2\u0303q\3\2\2\2\u0304\u0307"+
		"\5t;\2\u0305\u0307\5~@\2\u0306\u0304\3\2\2\2\u0306\u0305\3\2\2\2\u0307"+
		"s\3\2\2\2\u0308\u030d\5x=\2\u0309\u030d\5z>\2\u030a\u030d\5|?\2\u030b"+
		"\u030d\5v<\2\u030c\u0308\3\2\2\2\u030c\u0309\3\2\2\2\u030c\u030a\3\2\2"+
		"\2\u030c\u030b\3\2\2\2\u030du\3\2\2\2\u030e\u030f\7i\2\2\u030fw\3\2\2"+
		"\2\u0310\u0311\t\t\2\2\u0311y\3\2\2\2\u0312\u0313\t\n\2\2\u0313{\3\2\2"+
		"\2\u0314\u0315\7@\2\2\u0315\u0318\7@\2\2\u0316\u0318\7o\2\2\u0317\u0314"+
		"\3\2\2\2\u0317\u0316\3\2\2\2\u0318}\3\2\2\2\u0319\u031e\5\u0082B\2\u031a"+
		"\u031e\5\u0084C\2\u031b\u031e\5\u0086D\2\u031c\u031e\5\u0080A\2\u031d"+
		"\u0319\3\2\2\2\u031d\u031a\3\2\2\2\u031d\u031b\3\2\2\2\u031d\u031c\3\2"+
		"\2\2\u031e\177\3\2\2\2\u031f\u0320\7j\2\2\u0320\u0081\3\2\2\2\u0321\u0322"+
		"\7O\2\2\u0322\u0325\7?\2\2\u0323\u0325\7l\2\2\u0324\u0321\3\2\2\2\u0324"+
		"\u0323\3\2\2\2\u0325\u0083\3\2\2\2\u0326\u0327\7O\2\2\u0327\u032a\7@\2"+
		"\2\u0328\u032a\7n\2\2\u0329\u0326\3\2\2\2\u0329\u0328\3\2\2\2\u032a\u0085"+
		"\3\2\2\2\u032b\u032c\7O\2\2\u032c\u032d\7@\2\2\u032d\u0330\7@\2\2\u032e"+
		"\u0330\7p\2\2\u032f\u032b\3\2\2\2\u032f\u032e\3\2\2\2\u0330\u0087\3\2"+
		"\2\2\u0331\u0332\7T\2\2\u0332\u0089\3\2\2\2\u0333\u0334\7;\2\2\u0334\u008b"+
		"\3\2\2\2\u0335\u0336\7W\2\2\u0336\u008d\3\2\2\2\u0337\u0338\7\60\2\2\u0338"+
		"\u008f\3\2\2\2\u0339\u033a\7S\2\2\u033a\u0091\3\2\2\2\u033b\u033c\7M\2"+
		"\2\u033c\u0093\3\2\2\2\u033d\u0340\5\u0096L\2\u033e\u0340\5\u00a4S\2\u033f"+
		"\u033d\3\2\2\2\u033f\u033e\3\2\2\2\u0340\u0095\3\2\2\2\u0341\u0342\5\u0098"+
		"M\2\u0342\u0343\7\20\2\2\u0343\u0344\5\u009cO\2\u0344\u0345\7\21\2\2\u0345"+
		"\u0097\3\2\2\2\u0346\u0347\7q\2\2\u0347\u0349\7r\2\2\u0348\u034a\5\u009a"+
		"N\2\u0349\u0348\3\2\2\2\u0349\u034a\3\2\2\2\u034a\u0099\3\2\2\2\u034b"+
		"\u034c\7\16\2\2\u034c\u034d\5\30\r\2\u034d\u009b\3\2\2\2\u034e\u0350\5"+
		"\u009eP\2\u034f\u034e\3\2\2\2\u0350\u0353\3\2\2\2\u0351\u034f\3\2\2\2"+
		"\u0351\u0352\3\2\2\2\u0352\u009d\3\2\2\2\u0353\u0351\3\2\2\2\u0354\u035f"+
		"\5\u00a0Q\2\u0355\u0356\5\u00c2b\2\u0356\u0357\7\r\2\2\u0357\u035f\3\2"+
		"\2\2\u0358\u0359\5\66\34\2\u0359\u035a\7\r\2\2\u035a\u035f\3\2\2\2\u035b"+
		"\u035c\5T+\2\u035c\u035d\7\r\2\2\u035d\u035f\3\2\2\2\u035e\u0354\3\2\2"+
		"\2\u035e\u0355\3\2\2\2\u035e\u0358\3\2\2\2\u035e\u035b\3\2\2\2\u035f\u009f"+
		"\3\2\2\2\u0360\u0361\5\u00a2R\2\u0361\u0364\5l\67\2\u0362\u0363\7=\2\2"+
		"\u0363\u0365\5:\36\2\u0364\u0362\3\2\2\2\u0364\u0365\3\2\2\2\u0365\u0366"+
		"\3\2\2\2\u0366\u0367\7\r\2\2\u0367\u00a1\3\2\2\2\u0368\u036b\58\35\2\u0369"+
		"\u036b\5\u0090I\2\u036a\u0368\3\2\2\2\u036a\u0369\3\2\2\2\u036b\u00a3"+
		"\3\2\2\2\u036c\u036d\7q\2\2\u036d\u036e\7r\2\2\u036e\u00a5\3\2\2\2\u036f"+
		"\u0370\5\u013e\u00a0\2\u0370\u0371\7g\2\2\u0371\u0374\7r\2\2\u0372\u0373"+
		"\7\16\2\2\u0373\u0375\5\30\r\2\u0374\u0372\3\2\2\2\u0374\u0375\3\2\2\2"+
		"\u0375\u0376\3\2\2\2\u0376\u0378\7\20\2\2\u0377\u0379\5\u00a8U\2\u0378"+
		"\u0377\3\2\2\2\u0379\u037a\3\2\2\2\u037a\u0378\3\2\2\2\u037a\u037b\3\2"+
		"\2\2\u037b\u037c\3\2\2\2\u037c\u037d\7\21\2\2\u037d\u00a7\3\2\2\2\u037e"+
		"\u0380\5\u00aaV\2\u037f\u0381\5f\64\2\u0380\u037f\3\2\2\2\u0380\u0381"+
		"\3\2\2\2\u0381\u0382\3\2\2\2\u0382\u0383\7\r\2\2\u0383\u00a9\3\2\2\2\u0384"+
		"\u0385\5\u013e\u00a0\2\u0385\u0386\7f\2\2\u0386\u0387\7\30\2\2\u0387\u038a"+
		"\5P)\2\u0388\u0389\7\17\2\2\u0389\u038b\5^\60\2\u038a\u0388\3\2\2\2\u038a"+
		"\u038b\3\2\2\2\u038b\u038c\3\2\2\2\u038c\u038d\7\31\2\2\u038d\u00ab\3"+
		"\2\2\2\u038e\u038f\5\u013e\u00a0\2\u038f\u0390\7h\2\2\u0390\u0391\7r\2"+
		"\2\u0391\u0392\7\20\2\2\u0392\u0397\5\u00aeX\2\u0393\u0394\7\17\2\2\u0394"+
		"\u0396\5\u00aeX\2\u0395\u0393\3\2\2\2\u0396\u0399\3\2\2\2\u0397\u0395"+
		"\3\2\2\2\u0397\u0398\3\2\2\2\u0398\u039a\3\2\2\2\u0399\u0397\3\2\2\2\u039a"+
		"\u039b\7\21\2\2\u039b\u00ad\3\2\2\2\u039c\u039d\5\u013e\u00a0\2\u039d"+
		"\u039e\7r\2\2\u039e\u00af\3\2\2\2\u039f\u03a0\5\u013e\u00a0\2\u03a0\u03a1"+
		"\7\63\2\2\u03a1\u03a4\7r\2\2\u03a2\u03a3\7\16\2\2\u03a3\u03a5\5\30\r\2"+
		"\u03a4\u03a2\3\2\2\2\u03a4\u03a5\3\2\2\2\u03a5\u03a6\3\2\2\2\u03a6\u03a7"+
		"\7\20\2\2\u03a7\u03a8\5\u00b2Z\2\u03a8\u03a9\7\21\2\2\u03a9\u00b1\3\2"+
		"\2\2\u03aa\u03ac\5\u00b4[\2\u03ab\u03aa\3\2\2\2\u03ac\u03af\3\2\2\2\u03ad"+
		"\u03ab\3\2\2\2\u03ad\u03ae\3\2\2\2\u03ae\u00b3\3\2\2\2\u03af\u03ad\3\2"+
		"\2\2\u03b0\u03b1\5\u013e\u00a0\2\u03b1\u03b2\5Z.\2\u03b2\u03b3\5h\65\2"+
		"\u03b3\u03b4\7\r\2\2\u03b4\u00b5\3\2\2\2\u03b5\u03b6\5\u013e\u00a0\2\u03b6"+
		"\u03b7\7Q\2\2\u03b7\u03b8\7r\2\2\u03b8\u03b9\7*\2\2\u03b9\u03ba\7\22\2"+
		"\2\u03ba\u03bb\5\u00b8]\2\u03bb\u03bc\7\23\2\2\u03bc\u03bd\7\20\2\2\u03bd"+
		"\u03be\5\u00ba^\2\u03be\u03bf\7\21\2\2\u03bf\u00b7\3\2\2\2\u03c0\u03c8"+
		"\5r:\2\u03c1\u03c8\5\u0088E\2\u03c2\u03c8\5\u008aF\2\u03c3\u03c8\5\u008e"+
		"H\2\u03c4\u03c8\5\u008cG\2\u03c5\u03c8\5\u00c2b\2\u03c6\u03c8\5\30\r\2"+
		"\u03c7\u03c0\3\2\2\2\u03c7\u03c1\3\2\2\2\u03c7\u03c2\3\2\2\2\u03c7\u03c3"+
		"\3\2\2\2\u03c7\u03c4\3\2\2\2\u03c7\u03c5\3\2\2\2\u03c7\u03c6\3\2\2\2\u03c8"+
		"\u00b9\3\2\2\2\u03c9\u03cb\5\u00bc_\2\u03ca\u03c9\3\2\2\2\u03cb\u03cc"+
		"\3\2\2\2\u03cc\u03ca\3\2\2\2\u03cc\u03cd\3\2\2\2\u03cd\u00bb\3\2\2\2\u03ce"+
		"\u03d0\5\u00be`\2\u03cf\u03ce\3\2\2\2\u03d0\u03d1\3\2\2\2\u03d1\u03cf"+
		"\3\2\2\2\u03d1\u03d2\3\2\2\2\u03d2\u03d3\3\2\2\2\u03d3\u03d4\5\u00c0a"+
		"\2\u03d4\u03d5\7\r\2\2\u03d5\u00bd\3\2\2\2\u03d6\u03d7\7U\2\2\u03d7\u03d8"+
		"\5:\36\2\u03d8\u03d9\7\16\2\2\u03d9\u03dd\3\2\2\2\u03da\u03db\7=\2\2\u03db"+
		"\u03dd\7\16\2\2\u03dc\u03d6\3\2\2\2\u03dc\u03da\3\2\2\2\u03dd\u00bf\3"+
		"\2\2\2\u03de\u03df\5\u013e\u00a0\2\u03df\u03e0\5Z.\2\u03e0\u03e1\5j\66"+
		"\2\u03e1\u00c1\3\2\2\2\u03e2\u03e3\5\u013e\u00a0\2\u03e3\u03e4\7A\2\2"+
		"\u03e4\u03e5\7r\2\2\u03e5\u03e6\7\20\2\2\u03e6\u03eb\5\u00c4c\2\u03e7"+
		"\u03e8\7\17\2\2\u03e8\u03ea\5\u00c4c\2\u03e9\u03e7\3\2\2\2\u03ea\u03ed"+
		"\3\2\2\2\u03eb\u03e9\3\2\2\2\u03eb\u03ec\3\2\2\2\u03ec\u03ee\3\2\2\2\u03ed"+
		"\u03eb\3\2\2\2\u03ee\u03ef\7\21\2\2\u03ef\u00c3\3\2\2\2\u03f0\u03f1\5"+
		"\u013e\u00a0\2\u03f1\u03f2\7r\2\2\u03f2\u00c5\3\2\2\2\u03f3\u03f4\7\61"+
		"\2\2\u03f4\u03f5\7\30\2\2\u03f5\u03f8\5\\/\2\u03f6\u03f7\7\17\2\2\u03f7"+
		"\u03f9\5P)\2\u03f8\u03f6\3\2\2\2\u03f8\u03f9\3\2\2\2\u03f9\u03fa\3\2\2"+
		"\2\u03fa\u03fb\7\31\2\2\u03fb\u00c7\3\2\2\2\u03fc\u03fd\7e\2\2\u03fd\u03fe"+
		"\7\30\2\2\u03fe\u03ff\5\\/\2\u03ff\u0400\7\17\2\2\u0400\u0403\5\\/\2\u0401"+
		"\u0402\7\17\2\2\u0402\u0404\5P)\2\u0403\u0401\3\2\2\2\u0403\u0404\3\2"+
		"\2\2\u0404\u0405\3\2\2\2\u0405\u0406\7\31\2\2\u0406\u00c9\3\2\2\2\u0407"+
		"\u040c\7)\2\2\u0408\u0409\7\30\2\2\u0409\u040a\5P)\2\u040a\u040b\7\31"+
		"\2\2\u040b\u040d\3\2\2\2\u040c\u0408\3\2\2\2\u040c\u040d\3\2\2\2\u040d"+
		"\u00cb\3\2\2\2\u040e\u0413\7B\2\2\u040f\u0410\7\30\2\2\u0410\u0411\5P"+
		")\2\u0411\u0412\7\31\2\2\u0412\u0414\3\2\2\2\u0413\u040f\3\2\2\2\u0413"+
		"\u0414\3\2\2\2\u0414\u00cd\3\2\2\2\u0415\u0417\7r\2\2\u0416\u0418\5\u00d0"+
		"i\2\u0417\u0416\3\2\2\2\u0418\u0419\3\2\2\2\u0419\u0417\3\2\2\2\u0419"+
		"\u041a\3\2\2\2\u041a\u00cf\3\2\2\2\u041b\u041c\7\24\2\2\u041c\u041d\5"+
		"P)\2\u041d\u041e\7\25\2\2\u041e\u00d1\3\2\2\2\u041f\u0422\5\u013e\u00a0"+
		"\2\u0420\u0423\5\u00f8}\2\u0421\u0423\5\u00fc\177\2\u0422\u0420\3\2\2"+
		"\2\u0422\u0421\3\2\2\2\u0423\u00d3\3\2\2\2\u0424\u0425\5\u013e\u00a0\2"+
		"\u0425\u0426\7F\2\2\u0426\u0427\7r\2\2\u0427\u042b\7\20\2\2\u0428\u042a"+
		"\5\u00b4[\2\u0429\u0428\3\2\2\2\u042a\u042d\3\2\2\2\u042b\u0429\3\2\2"+
		"\2\u042b\u042c\3\2\2\2\u042c\u042e\3\2\2\2\u042d\u042b\3\2\2\2\u042e\u042f"+
		"\7\21\2\2\u042f\u00d5\3\2\2\2\u0430\u0432\5\u013e\u00a0\2\u0431\u0433"+
		"\5\u00d8m\2\u0432\u0431\3\2\2\2\u0432\u0433\3\2\2\2\u0433\u0434\3\2\2"+
		"\2\u0434\u0435\5\u00dan\2\u0435\u0436\7r\2\2\u0436\u0438\5\u00dco\2\u0437"+
		"\u0439\5\u00e2r\2\u0438\u0437\3\2\2\2\u0438\u0439\3\2\2\2\u0439\u043b"+
		"\3\2\2\2\u043a\u043c\5\u00e4s\2\u043b\u043a\3\2\2\2\u043b\u043c\3\2\2"+
		"\2\u043c\u00d7\3\2\2\2\u043d\u043e\7R\2\2\u043e\u00d9\3\2\2\2\u043f\u0442"+
		"\5\u00e6t\2\u0440\u0442\78\2\2\u0441\u043f\3\2\2\2\u0441\u0440\3\2\2\2"+
		"\u0442\u00db\3\2\2\2\u0443\u044c\7\22\2\2\u0444\u0449\5\u00dep\2\u0445"+
		"\u0446\7\17\2\2\u0446\u0448\5\u00dep\2\u0447\u0445\3\2\2\2\u0448\u044b"+
		"\3\2\2\2\u0449\u0447\3\2\2\2\u0449\u044a\3\2\2\2\u044a\u044d\3\2\2\2\u044b"+
		"\u0449\3\2\2\2\u044c\u0444\3\2\2\2\u044c\u044d\3\2\2\2\u044d\u044e\3\2"+
		"\2\2\u044e\u044f\7\23\2\2\u044f\u00dd\3\2\2\2\u0450\u0451\5\u013e\u00a0"+
		"\2\u0451\u0452\5\u00e0q\2\u0452\u0453\5\u00e6t\2\u0453\u0454\5l\67\2\u0454"+
		"\u00df\3\2\2\2\u0455\u0456\t\13\2\2\u0456\u00e1\3\2\2\2\u0457\u0458\7"+
		"\67\2\2\u0458\u0459\7\22\2\2\u0459\u045e\5\30\r\2\u045a\u045b\7\17\2\2"+
		"\u045b\u045d\5\30\r\2\u045c\u045a\3\2\2\2\u045d\u0460\3\2\2\2\u045e\u045c"+
		"\3\2\2\2\u045e\u045f\3\2\2\2\u045f\u0461\3\2\2\2\u0460\u045e\3\2\2\2\u0461"+
		"\u0462\7\23\2\2\u0462\u00e3\3\2\2\2\u0463\u0464\7C\2\2\u0464\u0465\7\22"+
		"\2\2\u0465\u046a\7\13\2\2\u0466\u0467\7\17\2\2\u0467\u0469\7\13\2\2\u0468"+
		"\u0466\3\2\2\2\u0469\u046c\3\2\2\2\u046a\u0468\3\2\2\2\u046a\u046b\3\2"+
		"\2\2\u046b\u046d\3\2\2\2\u046c\u046a\3\2\2\2\u046d\u046e\7\23\2\2\u046e"+
		"\u00e5\3\2\2\2\u046f\u0474\5`\61\2\u0470\u0474\5\u00caf\2\u0471\u0474"+
		"\5\u00ccg\2\u0472\u0474\5\30\r\2\u0473\u046f\3\2\2\2\u0473\u0470\3\2\2"+
		"\2\u0473\u0471\3\2\2\2\u0473\u0472\3\2\2\2\u0474\u00e7\3\2\2\2\u0475\u0476"+
		"\7P\2\2\u0476\u0477\7\30\2\2\u0477\u0478\5P)\2\u0478\u0479\7\17\2\2\u0479"+
		"\u047a\5P)\2\u047a\u047b\7\31\2\2\u047b\u00e9\3\2\2\2\u047c\u047d\7P\2"+
		"\2\u047d\u00eb\3\2\2\2\u047e\u047f\7I\2\2\u047f\u00ed\3\2\2\2\u0480\u0481"+
		"\7\63\2\2\u0481\u0485\7r\2\2\u0482\u0483\7Q\2\2\u0483\u0485\7r\2\2\u0484"+
		"\u0480\3\2\2\2\u0484\u0482\3\2\2\2\u0485\u00ef\3\2\2\2\u0486\u0487\7\62"+
		"\2\2\u0487\u0488\5\u00f2z\2\u0488\u00f1\3\2\2\2\u0489\u048c\5\30\r\2\u048a"+
		"\u048c\7\13\2\2\u048b\u0489\3\2\2\2\u048b\u048a\3\2\2\2\u048c\u00f3\3"+
		"\2\2\2\u048d\u048e\7_\2\2\u048e\u048f\5\30\r\2\u048f\u0490\7\13\2\2\u0490"+
		"\u00f5\3\2\2\2\u0491\u0492\7^\2\2\u0492\u0493\5\30\r\2\u0493\u0494\7\13"+
		"\2\2\u0494\u00f7\3\2\2\2\u0495\u0496\7\65\2\2\u0496\u0497\7`\2\2\u0497"+
		"\u0498\5\u00e6t\2\u0498\u0499\5\u00fa~\2\u0499\u00f9\3\2\2\2\u049a\u04a3"+
		"\5l\67\2\u049b\u04a4\5\u00e2r\2\u049c\u049d\7\17\2\2\u049d\u049f\5l\67"+
		"\2\u049e\u049c\3\2\2\2\u049f\u04a2\3\2\2\2\u04a0\u049e\3\2\2\2\u04a0\u04a1"+
		"\3\2\2\2\u04a1\u04a4\3\2\2\2\u04a2\u04a0\3\2\2\2\u04a3\u049b\3\2\2\2\u04a3"+
		"\u04a0\3\2\2\2\u04a4\u00fb\3\2\2\2\u04a5\u04a6\7`\2\2\u04a6\u04a7\5\u00e6"+
		"t\2\u04a7\u04a8\5\u00fe\u0080\2\u04a8\u00fd\3\2\2\2\u04a9\u04b2\5l\67"+
		"\2\u04aa\u04b3\5\u0100\u0081\2\u04ab\u04ac\7\17\2\2\u04ac\u04ae\5l\67"+
		"\2\u04ad\u04ab\3\2\2\2\u04ae\u04b1\3\2\2\2\u04af\u04ad\3\2\2\2\u04af\u04b0"+
		"\3\2\2\2\u04b0\u04b3\3\2\2\2\u04b1\u04af\3\2\2\2\u04b2\u04aa\3\2\2\2\u04b2"+
		"\u04af\3\2\2\2\u04b3\u00ff\3\2\2\2\u04b4\u04b6\5\u0102\u0082\2\u04b5\u04b7"+
		"\5\u0104\u0083\2\u04b6\u04b5\3\2\2\2\u04b6\u04b7\3\2\2\2\u04b7\u04ba\3"+
		"\2\2\2\u04b8\u04ba\5\u0104\u0083\2\u04b9\u04b4\3\2\2\2\u04b9\u04b8\3\2"+
		"\2\2\u04ba\u0101\3\2\2\2\u04bb\u04bc\7G\2\2\u04bc\u04bd\5\u0106\u0084"+
		"\2\u04bd\u0103\3\2\2\2\u04be\u04bf\7&\2\2\u04bf\u04c0\5\u0106\u0084\2"+
		"\u04c0\u0105\3\2\2\2\u04c1\u04c2\7\22\2\2\u04c2\u04c7\5\30\r\2\u04c3\u04c4"+
		"\7\17\2\2\u04c4\u04c6\5\30\r\2\u04c5\u04c3\3\2\2\2\u04c6\u04c9\3\2\2\2"+
		"\u04c7\u04c5\3\2\2\2\u04c7\u04c8\3\2\2\2\u04c8\u04ca\3\2\2\2\u04c9\u04c7"+
		"\3\2\2\2\u04ca\u04cb\7\23\2\2\u04cb\u0107\3\2\2\2\u04cc\u04cf\5\u010c"+
		"\u0087\2\u04cd\u04cf\5\u010a\u0086\2\u04ce\u04cc\3\2\2\2\u04ce\u04cd\3"+
		"\2\2\2\u04cf\u0109\3\2\2\2\u04d0\u04d1\5\u013e\u00a0\2\u04d1\u04d2\7d"+
		"\2\2\u04d2\u04d3\7r\2\2\u04d3\u010b\3\2\2\2\u04d4\u04d5\5\u013e\u00a0"+
		"\2\u04d5\u04d6\5\u010e\u0088\2\u04d6\u04d7\7\20\2\2\u04d7\u04d8\5\u0114"+
		"\u008b\2\u04d8\u04d9\7\21\2\2\u04d9\u010d\3\2\2\2\u04da\u04db\7d\2\2\u04db"+
		"\u04dd\7r\2\2\u04dc\u04de\5\u0112\u008a\2\u04dd\u04dc\3\2\2\2\u04dd\u04de"+
		"\3\2\2\2\u04de\u04e0\3\2\2\2\u04df\u04e1\5\u0110\u0089\2\u04e0\u04df\3"+
		"\2\2\2\u04e0\u04e1\3\2\2\2\u04e1\u010f\3\2\2\2\u04e2\u04e3\7K\2\2\u04e3"+
		"\u04e8\5\30\r\2\u04e4\u04e5\7\17\2\2\u04e5\u04e7\5\30\r\2\u04e6\u04e4"+
		"\3\2\2\2\u04e7\u04ea\3\2\2\2\u04e8\u04e6\3\2\2\2\u04e8\u04e9\3\2\2\2\u04e9"+
		"\u0111\3\2\2\2\u04ea\u04e8\3\2\2\2\u04eb\u04ec\7\16\2\2\u04ec\u04ed\5"+
		"\30\r\2\u04ed\u0113\3\2\2\2\u04ee\u04f0\5\u0116\u008c\2\u04ef\u04ee\3"+
		"\2\2\2\u04f0\u04f3\3\2\2\2\u04f1\u04ef\3\2\2\2\u04f1\u04f2\3\2\2\2\u04f2"+
		"\u0115\3\2\2\2\u04f3\u04f1\3\2\2\2\u04f4\u0507\5\u013e\u00a0\2\u04f5\u04f6"+
		"\5\u0118\u008d\2\u04f6\u04f7\7\r\2\2\u04f7\u0508\3\2\2\2\u04f8\u04f9\5"+
		"\u011c\u008f\2\u04f9\u04fa\7\r\2\2\u04fa\u0508\3\2\2\2\u04fb\u04fc\5\u011e"+
		"\u0090\2\u04fc\u04fd\7\r\2\2\u04fd\u0508\3\2\2\2\u04fe\u04ff\5\u0120\u0091"+
		"\2\u04ff\u0500\7\r\2\2\u0500\u0508\3\2\2\2\u0501\u0502\5\u0122\u0092\2"+
		"\u0502\u0503\7\r\2\2\u0503\u0508\3\2\2\2\u0504\u0505\5\u00d2j\2\u0505"+
		"\u0506\7\r\2\2\u0506\u0508\3\2\2\2\u0507\u04f5\3\2\2\2\u0507\u04f8\3\2"+
		"\2\2\u0507\u04fb\3\2\2\2\u0507\u04fe\3\2\2\2\u0507\u0501\3\2\2\2\u0507"+
		"\u0504\3\2\2\2\u0508\u0117\3\2\2\2\u0509\u050a\7[\2\2\u050a\u050b\5\u011a"+
		"\u008e\2\u050b\u050c\7r\2\2\u050c\u0119\3\2\2\2\u050d\u0510\5\30\r\2\u050e"+
		"\u0510\7M\2\2\u050f\u050d\3\2\2\2\u050f\u050e\3\2\2\2\u0510\u011b\3\2"+
		"\2\2\u0511\u0513\7-\2\2\u0512\u0514\7X\2\2\u0513\u0512\3\2\2\2\u0513\u0514"+
		"\3\2\2\2\u0514\u0515\3\2\2\2\u0515\u0516\5\u011a\u008e\2\u0516\u0517\7"+
		"r\2\2\u0517\u011d\3\2\2\2\u0518\u0519\7(\2\2\u0519\u051a\5\30\r\2\u051a"+
		"\u051b\7r\2\2\u051b\u011f\3\2\2\2\u051c\u051d\7+\2\2\u051d\u051e\5\30"+
		"\r\2\u051e\u051f\7r\2\2\u051f\u0121\3\2\2\2\u0520\u0521\7\\\2\2\u0521"+
		"\u0522\5\30\r\2\u0522\u0523\7r\2\2\u0523\u0123\3\2\2\2\u0524\u0525\5\u0126"+
		"\u0094\2\u0525\u0526\5\u012c\u0097\2\u0526\u0125\3\2\2\2\u0527\u0528\7"+
		"D\2\2\u0528\u052a\7r\2\2\u0529\u052b\5\u0128\u0095\2\u052a\u0529\3\2\2"+
		"\2\u052a\u052b\3\2\2\2\u052b\u052d\3\2\2\2\u052c\u052e\5\u0110\u0089\2"+
		"\u052d\u052c\3\2\2\2\u052d\u052e\3\2\2\2\u052e\u052f\3\2\2\2\u052f\u0530"+
		"\7b\2\2\u0530\u0532\5\30\r\2\u0531\u0533\5\u012a\u0096\2\u0532\u0531\3"+
		"\2\2\2\u0532\u0533\3\2\2\2\u0533\u0127\3\2\2\2\u0534\u0535\7\16\2\2\u0535"+
		"\u0536\5\30\r\2\u0536\u0129\3\2\2\2\u0537\u0538\7.\2\2\u0538\u0539\5\30"+
		"\r\2\u0539\u012b\3\2\2\2\u053a\u053e\7\20\2\2\u053b\u053d\5\u012e\u0098"+
		"\2\u053c\u053b\3\2\2\2\u053d\u0540\3\2\2\2\u053e\u053c\3\2\2\2\u053e\u053f"+
		"\3\2\2\2\u053f\u0541\3\2\2\2\u0540\u053e\3\2\2\2\u0541\u0542\7\21\2\2"+
		"\u0542\u012d\3\2\2\2\u0543\u054c\5\22\n\2\u0544\u0547\5\u013e\u00a0\2"+
		"\u0545\u0548\5\u0130\u0099\2\u0546\u0548\5\u0132\u009a\2\u0547\u0545\3"+
		"\2\2\2\u0547\u0546\3\2\2\2\u0548\u0549\3\2\2\2\u0549\u054a\7\r\2\2\u054a"+
		"\u054c\3\2\2\2\u054b\u0543\3\2\2\2\u054b\u0544\3\2\2\2\u054c\u012f\3\2"+
		"\2\2\u054d\u054e\7E\2\2\u054e\u054f\7r\2\2\u054f\u0551\7\22\2\2\u0550"+
		"\u0552\5\60\31\2\u0551\u0550\3\2\2\2\u0551\u0552\3\2\2\2\u0552\u0553\3"+
		"\2\2\2\u0553\u0555\7\23\2\2\u0554\u0556\5\u00e2r\2\u0555\u0554\3\2\2\2"+
		"\u0555\u0556\3\2\2\2\u0556\u0131\3\2\2\2\u0557\u0558\7\66\2\2\u0558\u0559"+
		"\7r\2\2\u0559\u055b\7\22\2\2\u055a\u055c\5\60\31\2\u055b\u055a\3\2\2\2"+
		"\u055b\u055c\3\2\2\2\u055c\u055d\3\2\2\2\u055d\u055f\7\23\2\2\u055e\u0560"+
		"\5\u00e2r\2\u055f\u055e\3\2\2\2\u055f\u0560\3\2\2\2\u0560\u0133\3\2\2"+
		"\2\u0561\u0565\5\u013e\u00a0\2\u0562\u0566\5\u013a\u009e\2\u0563\u0566"+
		"\5\u0138\u009d\2\u0564\u0566\5\u0136\u009c\2\u0565\u0562\3\2\2\2\u0565"+
		"\u0563\3\2\2\2\u0565\u0564\3\2\2\2\u0566\u0135\3\2\2\2\u0567\u0569\7Y"+
		"\2\2\u0568\u0567\3\2\2\2\u0568\u0569\3\2\2\2\u0569\u056a\3\2\2\2\u056a"+
		"\u056b\7:\2\2\u056b\u056c\7r\2\2\u056c\u0137\3\2\2\2\u056d\u056e\7Y\2"+
		"\2\u056e\u056f\7:\2\2\u056f\u0570\7r\2\2\u0570\u0571\5&\24\2\u0571\u0575"+
		"\7\20\2\2\u0572\u0574\5\22\n\2\u0573\u0572\3\2\2\2\u0574\u0577\3\2\2\2"+
		"\u0575\u0573\3\2\2\2\u0575\u0576\3\2\2\2\u0576\u0578\3\2\2\2\u0577\u0575"+
		"\3\2\2\2\u0578\u0579\7\21\2\2\u0579\u0139\3\2\2\2\u057a\u057b\5\u013c"+
		"\u009f\2\u057b\u057f\7\20\2\2\u057c\u057e\5*\26\2\u057d\u057c\3\2\2\2"+
		"\u057e\u0581\3\2\2\2\u057f\u057d\3\2\2\2\u057f\u0580\3\2\2\2\u0580\u0582"+
		"\3\2\2\2\u0581\u057f\3\2\2\2\u0582\u0583\7\21\2\2\u0583\u013b\3\2\2\2"+
		"\u0584\u0586\7/\2\2\u0585\u0584\3\2\2\2\u0585\u0586\3\2\2\2\u0586\u0587"+
		"\3\2\2\2\u0587\u0588\7:\2\2\u0588\u0589\7r\2\2\u0589\u058a\5&\24\2\u058a"+
		"\u013d\3\2\2\2\u058b\u058d\5\u0140\u00a1\2\u058c\u058b\3\2\2\2\u058d\u0590"+
		"\3\2\2\2\u058e\u058c\3\2\2\2\u058e\u058f\3\2\2\2\u058f\u013f\3\2\2\2\u0590"+
		"\u058e\3\2\2\2\u0591\u0592\7%\2\2\u0592\u0597\5\30\r\2\u0593\u0594\7\22"+
		"\2\2\u0594\u0595\5\u0142\u00a2\2\u0595\u0596\7\23\2\2\u0596\u0598\3\2"+
		"\2\2\u0597\u0593\3\2\2\2\u0597\u0598\3\2\2\2\u0598\u0141\3\2\2\2\u0599"+
		"\u05a3\5:\36\2\u059a\u059f\5\u0144\u00a3\2\u059b\u059c\7\17\2\2\u059c"+
		"\u059e\5\u0144\u00a3\2\u059d\u059b\3\2\2\2\u059e\u05a1\3\2\2\2\u059f\u059d"+
		"\3\2\2\2\u059f\u05a0\3\2\2\2\u05a0\u05a3\3\2\2\2\u05a1\u059f\3\2\2\2\u05a2"+
		"\u0599\3\2\2\2\u05a2\u059a\3\2\2\2\u05a3\u0143\3\2\2\2\u05a4\u05a5\7r"+
		"\2\2\u05a5\u05a6\7 \2\2\u05a6\u05a7\5:\36\2\u05a7\u0145\3\2\2\2\u05a8"+
		"\u05a9\7|\2\2\u05a9\u05b3\7\13\2\2\u05aa\u05ab\7}\2\2\u05ab\u05ac\5\30"+
		"\r\2\u05ac\u05ad\7\13\2\2\u05ad\u05b3\3\2\2\2\u05ae\u05af\7{\2\2\u05af"+
		"\u05b0\5\30\r\2\u05b0\u05b1\7v\2\2\u05b1\u05b3\3\2\2\2\u05b2\u05a8\3\2"+
		"\2\2\u05b2\u05aa\3\2\2\2\u05b2\u05ae\3\2\2\2\u05b3\u0147\3\2\2\2\u05b4"+
		"\u05b5\7y\2\2\u05b5\u05b6\7\3\2\2\u05b6\u05b7\7\13\2\2\u05b7\u0149\3\2"+
		"\2\2\u05b8\u05b9\7w\2\2\u05b9\u05ba\7x\2\2\u05ba\u05bb\5\u014e\u00a8\2"+
		"\u05bb\u05bc\7\u00a0\2\2\u05bc\u05c2\3\2\2\2\u05bd\u05bf\5\u00f0y\2\u05be"+
		"\u05c0\7\r\2\2\u05bf\u05be\3\2\2\2\u05bf\u05c0\3\2\2\2\u05c0\u05c2\3\2"+
		"\2\2\u05c1\u05b8\3\2\2\2\u05c1\u05bd\3\2\2\2\u05c2\u014b\3\2\2\2\u05c3"+
		"\u05c4\7\u0080\2\2\u05c4\u05d8\5\u0150\u00a9\2\u05c5\u05c6\7\u0081\2\2"+
		"\u05c6\u05d8\5\u0150\u00a9\2\u05c7\u05d8\7\u0082\2\2\u05c8\u05ca\7\u0086"+
		"\2\2\u05c9\u05cb\5\u014e\u00a8\2\u05ca\u05c9\3\2\2\2\u05ca\u05cb\3\2\2"+
		"\2\u05cb\u05d8\3\2\2\2\u05cc\u05cd\7\u0084\2\2\u05cd\u05d8\7\u0097\2\2"+
		"\u05ce\u05cf\7\u0085\2\2\u05cf\u05d8\7\u0097\2\2\u05d0\u05d1\7\u0083\2"+
		"\2\u05d1\u05d8\7\u0097\2\2\u05d2\u05d3\7~\2\2\u05d3\u05d5\7\u0097\2\2"+
		"\u05d4\u05d6\5\u014e\u00a8\2\u05d5\u05d4\3\2\2\2\u05d5\u05d6\3\2\2\2\u05d6"+
		"\u05d8\3\2\2\2\u05d7\u05c3\3\2\2\2\u05d7\u05c5\3\2\2\2\u05d7\u05c7\3\2"+
		"\2\2\u05d7\u05c8\3\2\2\2\u05d7\u05cc\3\2\2\2\u05d7\u05ce\3\2\2\2\u05d7"+
		"\u05d0\3\2\2\2\u05d7\u05d2\3\2\2\2\u05d8\u014d\3\2\2\2\u05d9\u05db\7\u009f"+
		"\2\2\u05da\u05d9\3\2\2\2\u05db\u05dc\3\2\2\2\u05dc\u05da\3\2\2\2\u05dc"+
		"\u05dd\3\2\2\2\u05dd\u014f\3\2\2\2\u05de\u05df\b\u00a9\1\2\u05df\u05f8"+
		"\7\u0087\2\2\u05e0\u05f8\7\u0088\2\2\u05e1\u05f8\7\u0098\2\2\u05e2\u05f8"+
		"\7\u0096\2\2\u05e3\u05e8\7\u0097\2\2\u05e4\u05e5\7\u008b\2\2\u05e5\u05e6"+
		"\5\u0150\u00a9\2\u05e6\u05e7\7\u008c\2\2\u05e7\u05e9\3\2\2\2\u05e8\u05e4"+
		"\3\2\2\2\u05e8\u05e9\3\2\2\2\u05e9\u05f8\3\2\2\2\u05ea\u05eb\7\u008b\2"+
		"\2\u05eb\u05ec\5\u0150\u00a9\2\u05ec\u05ed\7\u008c\2\2\u05ed\u05f8\3\2"+
		"\2\2\u05ee\u05ef\7\u008a\2\2\u05ef\u05f8\5\u0150\u00a9\b\u05f0\u05f5\7"+
		"\177\2\2\u05f1\u05f6\7\u0097\2\2\u05f2\u05f3\7\u008b\2\2\u05f3\u05f4\7"+
		"\u0097\2\2\u05f4\u05f6\7\u008c\2\2\u05f5\u05f1\3\2\2\2\u05f5\u05f2\3\2"+
		"\2\2\u05f6\u05f8\3\2\2\2\u05f7\u05de\3\2\2\2\u05f7\u05e0\3\2\2\2\u05f7"+
		"\u05e1\3\2\2\2\u05f7\u05e2\3\2\2\2\u05f7\u05e3\3\2\2\2\u05f7\u05ea\3\2"+
		"\2\2\u05f7\u05ee\3\2\2\2\u05f7\u05f0\3\2\2\2\u05f8\u0607\3\2\2\2\u05f9"+
		"\u05fa\f\7\2\2\u05fa\u05fb\t\f\2\2\u05fb\u0606\5\u0150\u00a9\b\u05fc\u05fd"+
		"\f\6\2\2\u05fd\u05fe\7\u008f\2\2\u05fe\u0606\5\u0150\u00a9\7\u05ff\u0600"+
		"\f\5\2\2\u0600\u0601\7\u0090\2\2\u0601\u0606\5\u0150\u00a9\6\u0602\u0603"+
		"\f\4\2\2\u0603\u0604\t\r\2\2\u0604\u0606\5\u0150\u00a9\5\u0605\u05f9\3"+
		"\2\2\2\u0605\u05fc\3\2\2\2\u0605\u05ff\3\2\2\2\u0605\u0602\3\2\2\2\u0606"+
		"\u0609\3\2\2\2\u0607\u0605\3\2\2\2\u0607\u0608\3\2\2\2\u0608\u0151\3\2"+
		"\2\2\u0609\u0607\3\2\2\2\u008d\u0155\u016b\u016f\u0186\u018f\u0196\u019e"+
		"\u01a4\u01a9\u01ae\u01c2\u01c6\u01c8\u01d0\u01d6\u01dd\u01e5\u01e8\u01f9"+
		"\u0203\u0209\u0211\u0218\u021b\u0223\u0226\u022d\u023a\u023e\u0247\u0262"+
		"\u026b\u0273\u027b\u0283\u028b\u0293\u029a\u02a4\u02b2\u02c1\u02c6\u02cb"+
		"\u02d6\u02dd\u02e4\u02eb\u02f3\u02f8\u0302\u0306\u030c\u0317\u031d\u0324"+
		"\u0329\u032f\u033f\u0349\u0351\u035e\u0364\u036a\u0374\u037a\u0380\u038a"+
		"\u0397\u03a4\u03ad\u03c7\u03cc\u03d1\u03dc\u03eb\u03f8\u0403\u040c\u0413"+
		"\u0419\u0422\u042b\u0432\u0438\u043b\u0441\u0449\u044c\u045e\u046a\u0473"+
		"\u0484\u048b\u04a0\u04a3\u04af\u04b2\u04b6\u04b9\u04c7\u04ce\u04dd\u04e0"+
		"\u04e8\u04f1\u0507\u050f\u0513\u052a\u052d\u0532\u053e\u0547\u054b\u0551"+
		"\u0555\u055b\u055f\u0565\u0568\u0575\u057f\u0585\u058e\u0597\u059f\u05a2"+
		"\u05b2\u05bf\u05c1\u05ca\u05d5\u05d7\u05dc\u05e8\u05f5\u05f7\u0605\u0607";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}