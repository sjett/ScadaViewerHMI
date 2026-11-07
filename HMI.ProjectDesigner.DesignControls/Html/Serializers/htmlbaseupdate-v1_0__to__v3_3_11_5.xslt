<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes" cdata-section-elements="ParameterName Name Value"/>

  <xsl:template match="Attributes">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
      <BorderThickness>
        <xsl:value-of select="BorderThicknessLeft"/>,<xsl:value-of select="BorderThicknessTop"/>,<xsl:value-of select="BorderThicknessRight"/>,<xsl:value-of select="BorderThicknessBottom"/>
      </BorderThickness>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="BorderThicknessTop|BorderThicknessRight|BorderThicknessBottom|BorderThicknessLeft"/>
  
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>