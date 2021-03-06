[![Build status](https://ci.appveyor.com/api/projects/status/ly4kn3f900aw8i90/branch/master?svg=true)](https://ci.appveyor.com/project/IvanIakimov/rational-numbers/branch/master)

# Rational Numbers library

This library implements structure for presentation of rational numbers in .NET. The implementation is based on numerator and denomination which are presented by two 'long' values. Denominator is always positive and numerator can have any sign. Rational numbers in the library are presented in the form of irreducible fractions. Algorithm for calculation of greatest common divisor is used to reduce numbers into this form.

 In some operations overflow may happen. In this case System.OverflowException will be thrown.

 The following operations are supported by the library:

* \+
* \-
* \*
* /
* \>
* \<
* \>=
* \<=
* ==
* \!=
* absolute value
* minimum and maximum of two values
* implicit convertion from integer values (byte, int, long, ...)
* explicit convertion to double, decimal and float

RationalNumber structure implements IEquatable and IComparable interfaces.