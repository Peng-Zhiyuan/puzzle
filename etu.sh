cd `dirname $0`

cp -r excel excel.temp
etucli ./excel.temp
cp ./excel.temp/etu.json Assets/Resources/static-data-lite/etu.json
rm -r excel.temp