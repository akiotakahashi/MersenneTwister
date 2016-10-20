@echo off
mkdir ..\nuget >NUL 2>&1

set LICENSE=..\nuget\LICENSES-MersenneTwister.txt
echo THIS FILE IS A COLLECTION OF THE LICENSES RELATED TO MERSENNE TWISTER PACKAGE.> %LICENSE%
type separator.txt                                >> %LICENSE%
echo Mersenne Twister Pseudorandom Number Generator .NET Library>> %LICENSE%
echo ----------------------------------------------------------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE.txt               >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo MT19937ar>> %LICENSE%
echo --------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE-mt19937ar.txt     >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo MT19937ar-cok>> %LICENSE%
echo ------------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE-mt19937ar-cok.txt >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo MT19937-64>> %LICENSE%
echo ---------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE-mt19937-64.txt    >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo SFMT 1.4.1>> %LICENSE%
echo ---------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE-SFMT-1.4.1.txt    >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo dSFMT 2.2.3>> %LICENSE%
echo ----------->> %LICENSE%
echo;>> %LICENSE%
type ..\MersenneTwister\LICENSE-dSFMT-2.2.3.txt   >> %LICENSE%
type separator.txt                                >> %LICENSE%
echo (EOF)>> %LICENSE%

nuget pack -Build -BasePath ..\MersenneTwister -OutputDirectory ..\nuget ..\MersenneTwister\MersenneTwister.nuspec
pause
