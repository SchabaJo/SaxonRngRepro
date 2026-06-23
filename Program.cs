using Saxon.Api;
using Saxon.Hej.value;

var nsUri = @"http://org.SchabaJo.SaxonRngRepro";
var nsPrefix = "ns0";
var fnCurrent = "random-currentDateTime";
var fnConst = "random-constDateTime";
var fnString = "random-stringDateTime";
var docStr = $"""
<xsl:stylesheet version="3.0" 
                exclude-result-prefixes="xs {nsPrefix}"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:{nsPrefix}="{nsUri}">

    <xsl:function name="{nsPrefix}:{fnCurrent}" as="xs:double" visibility="public">
        <xsl:param name="input" as="xs:double?"/>

        <xsl:variable name="value" as="xs:double">
            <xsl:choose>
                <xsl:when test="empty($input) or $input = 0 or number($input) != number($input)">
                    <xsl:sequence select="random-number-generator(current-dateTime())?number"/>
                </xsl:when>

                <xsl:when test="$input &lt; 0">
                    <xsl:sequence select="$input * -1"/>
                </xsl:when>

                <xsl:otherwise>
                    <xsl:sequence select="$input"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>

        <xsl:choose>
            <xsl:when test="$value &lt; 10000">
                <xsl:sequence select="{nsPrefix}:{fnCurrent}($value * 10)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:sequence select="floor($value)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:function>

    <xsl:function name="{nsPrefix}:{fnConst}" as="xs:double" visibility="public">
        <xsl:param name="input" as="xs:double?"/>

        <xsl:variable name="value" as="xs:double">
            <xsl:choose>
                <xsl:when test="empty($input) or $input = 0 or number($input) != number($input)">
                    <xsl:sequence select="random-number-generator(xs:dateTime('2025-02-16T09:35:47Z'))?number"/>
                </xsl:when>

                <xsl:when test="$input &lt; 0">
                    <xsl:sequence select="$input * -1"/>
                </xsl:when>

                <xsl:otherwise>
                    <xsl:sequence select="$input"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>

        <xsl:choose>
            <xsl:when test="$value &lt; 10000">
                <xsl:sequence select="{nsPrefix}:{fnConst}($value * 10)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:sequence select="floor($value)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:function>

    <xsl:function name="{nsPrefix}:{fnString}" as="xs:double" visibility="public">
        <xsl:param name="input" as="xs:double?"/>

        <xsl:variable name="value" as="xs:double">
            <xsl:choose>
                <xsl:when test="empty($input) or $input = 0 or number($input) != number($input)">
                    <xsl:sequence select="random-number-generator(string(current-dateTime()))?number"/>
                </xsl:when>

                <xsl:when test="$input &lt; 0">
                    <xsl:sequence select="$input * -1"/>
                </xsl:when>

                <xsl:otherwise>
                    <xsl:sequence select="$input"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>

        <xsl:choose>
            <xsl:when test="$value &lt; 10000">
                <xsl:sequence select="{nsPrefix}:{fnString}($value * 10)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:sequence select="floor($value)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:function>
</xsl:stylesheet>
""";

var pinnedDateTime = new DateTimeValue(2025, 2, 16, 9, 35, 47, 0);
var parameters = new XdmValue[] { XdmEmptySequence.Instance };
var fnCurrentQN = new QName(nsUri, fnCurrent);
var fnConstQN = new QName(nsUri, fnConst);
var fnStringQN = new QName(nsUri, fnString);
using var reader = new StringReader(docStr);

var processor = new Processor();
var docBuilder = processor.NewDocumentBuilder();
var compiler = processor.NewXsltCompiler();
var docXdm = docBuilder.Build(reader);
var transformer = compiler.Compile(docXdm).Load30();
var controller = transformer.GetUnderlyingController;
controller.setCurrentDateTime(pinnedDateTime);

Console.WriteLine($"{fnCurrent} 1:\t{transformer.CallFunction(fnCurrentQN, parameters)}");
Console.WriteLine($"{fnCurrent} 2:\t{transformer.CallFunction(fnCurrentQN, parameters)}");
Console.WriteLine($"{fnCurrent} 3:\t{transformer.CallFunction(fnCurrentQN, parameters)}");

Console.WriteLine($"{fnConst} 1:\t\t{transformer.CallFunction(fnConstQN, parameters)}");
Console.WriteLine($"{fnConst} 2:\t\t{transformer.CallFunction(fnConstQN, parameters)}");
Console.WriteLine($"{fnConst} 3:\t\t{transformer.CallFunction(fnConstQN, parameters)}");

Console.WriteLine($"{fnString} 1:\t{transformer.CallFunction(fnStringQN, parameters)}");
Console.WriteLine($"{fnString} 2:\t{transformer.CallFunction(fnStringQN, parameters)}");
Console.WriteLine($"{fnString} 3:\t{transformer.CallFunction(fnStringQN, parameters)}");
