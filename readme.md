# Global Payments HPP .NET

[![Build status](https://ci.appveyor.com/api/projects/status/3d893l6i8yylfvnm/branch/master?svg=true)](https://ci.appveyor.com/project/JasonElkin25959/globalpayments-hpp-dotnet/branch/master)

A  small .NET (Framework) library for creating the JSON used by the [JavaScript HPP SDK](https://github.com/globalpayments/rxp-js) and for parsing the subsequent response(s) as per the [documentation](https://developer.globalpay.com/#!/hpp/getting-started).

## This is *not* the official .NET SDK

You should take a look the [official .NET SDK](https://github.com/globalpayments/dotnet-sdk) first and make sure that's not a better fit for you.

### So why is this a thing?

Firstly, history. This library started out before the official .NET SDK existed and was reverse engineered from the old PHP SDK.

Secondly, compatibility. The official .NET SDK has some dependencies that we can't use in certain projects at Semantic.

## 3D Secure v2

This library is for [Version 2 of the HPP API](https://developer.globalpay.com/#!/ecommerce/3d-secure-version2) which supports 3D Secure v2, but this has not yet been tested in production (hence this is an RC).
