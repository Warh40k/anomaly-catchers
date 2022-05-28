import pandas as pd
from duplicate_functions import drop_dup_ext2, drop_dup_ext1
from anomaly_functions import repeat_anomaly
import json

# здесь импортируем данные, на которых хотим проверить аномалии
# 100 000 строк ext2 обрабатываются примерно 2 минуты (не стоит кидать всю выборку)
ext2 = pd.read_csv('Датасет\\db2\\Ext2.csv')
ext1 = pd.read_csv('Датасет\\db2\\Ext.csv')
catch = pd.read_csv('Датасет\\db1\\catch.csv') 
product = pd.read_csv('Датасет\\db1\\product.csv') 

# первичная обработка данных
ext1.rename(columns={'id_fishery':'id_ves'}, inplace=True)
catch.date = pd.to_datetime(catch.date)
product.date = pd.to_datetime(product.date)
ext1.date_fishery = pd.to_datetime(ext1.date_fishery) 
ext2.date_vsd = pd.to_datetime(ext2.date_vsd) 

# убрать дубликаты в ext2 и подобных файлах
ext2 = drop_dup_ext2(ext2)

# убрать дубликаты в ext1 и подобных файлах
ext1 = drop_dup_ext1(ext1)

# вот здесь выборку по периоду можно задать
date_from = '2022-04-15'
date_to = '2022-04-20'

samp_1 = ext1[(ext1.date_fishery >= pd.to_datetime(date_from)) & (ext1.date_fishery <= pd.to_datetime(date_to))]
samp_2 = ext2[(ext2.date_vsd >= pd.to_datetime(date_from)) & (ext2.date_vsd <= pd.to_datetime(date_to))]

# поиск аномалии ошибки в единицах измерения и отправке повторного отчета
# нужен данные, подобный ext2

repeat_anomaly(samp_2) # json_file='delay_anomaly_report.json' - по умолчанию