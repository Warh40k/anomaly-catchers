import json
import pandas as pd
from tqdm import tqdm
def repeat_anomaly(ext2, txt_file='anomaly.txt', hours_delay=1):
    anomaly_count = 0
    anomaly_dict = {'violation':[], 'mistake':[]}
    tmp = ext2[ext2.volume > 0].sort_values(by='volume')
    for i in tqdm(range(tmp.shape[0] - 1)):
        lines = tmp.iloc[i:i+2]
        c1 = lines.volume.iloc[0] == lines.volume.iloc[1]
        c2 = lines.unit.iloc[0] != lines.unit.iloc[1]
        c3 = pd.Timedelta(lines.date_vsd.iloc[0] - lines.date_vsd.iloc[1]).seconds / 3600.0 < hours_delay
        c4 = lines.fish.iloc[0] == lines.fish.iloc[1]
        lines.date_vsd = lines.date_vsd.astype(str)
        if c1 & c2 & c3 & c4:
            anomaly_dict['mistake'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json()) 
            anomaly_count += 1         
        if (c1 & c2 & c4) and not c3:
            anomaly_dict['violation'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json())          
            anomaly_count += 1         

    print(txt_file)
    # Человекочитаемый вид
    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях repeat_report\n')
        print(f'Выявлено {anomaly_count} аномалий\n', file=f)
        print(f'Задержки в пределах допустимого:\n', file=f)
        for line in anomaly_dict['mistake']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f) 
        print(file=f)
        print('-' * 40, file=f)
        print(file=f)
        print(f'Задержки больше {hours_delay} часа(ов):\n', file=f)
        for line in anomaly_dict['violation']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f)

    return anomaly_dict, anomaly_count
    
def duplicate_anomaly(ext1, ext2, txt_file = 'anomaly.txt'):

    # df2 = ext2.copy()
    # initial_drop = df2[df2.duplicated(['id_vsd', 'fish'], keep=False)].index
    # df2 = df2.drop(initial_drop)
    # tmp = df2[df2.duplicated('id_vsd', keep=False)]
    # drop_idx = tmp.sort_values(by=['id_vsd', 'unit']).index[1::2]
    # tmp = tmp.drop(drop_idx)

    df1 = ext1.copy()
    tmp1 = df1[df1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves'])
    change_idx = df1[df1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves']).index
    df1.loc[change_idx, 'Product_period'] = (tmp1.Product_period + ' - ' + tmp1.date_fishery.astype(str)).values
    df1.loc[change_idx[::2], 'id_ves'] = (df1.loc[change_idx[1::2], 'id_ves']).values
    drop_idx = df1[df1.duplicated(['id_vsd'], keep='first')].sort_values(by=['id_vsd', 'id_ves']).index
    df1 = df1.loc[drop_idx]

    # Человекочитаемый вид
    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях duplicate_anomaly\n', file=f)
        print(f'Выявлено {len(df1)} аномалий (дубликатов)\n', file=f)
        print(f'Владельцам следующих кораблей были отправлены предупреждения:\n', file=f)
        print(df1[['id_own', 'id_ves']].sort_values(by=['id_own', 'id_ves']).value_counts().to_string(), file=f) 


    return df1[['id_ves', 'id_own', 'date_fishery']].to_json(), len(df1)

    duplicate, anomaly_amount

def vsd_absence_anomaly(Ext, Ext2, catch, txt_file='anomaly.txt'):
    A = Ext[['id_own','id_vsd','date_fishery']]
    B = Ext2[['id_vsd','id_fish','date_vsd','fish']]
    db2 = A.merge (B, on='id_vsd', how='inner')
    db2_fish = db2[['id_own','id_fish']].groupby(['id_own'])
    db2_fish = db2_fish.aggregate(lambda x: x.unique().tolist())

    db1_fish = catch[['id_own','id_fish']].groupby(['id_own'])
    db1_fish = db1_fish.aggregate(lambda x: x.unique().tolist())
    db12 = db1_fish.merge(db2_fish, on='id_own')
    db12['div_sub_vsd'] = None
    db12['div_sub_catch'] = None
    for i in (db12.index):
        db12.loc[i]['div_sub_vsd'] = set(db12.id_fish_y.loc[i]) - set(db12.id_fish_x.loc[i])
        db12.loc[i]['div_sub_catch'] = set(db12.id_fish_x.loc[i]) - set(db12.id_fish_y.loc[i])
    bad_fish_catch = db12[db12['div_sub_catch'] != set()]
    bad_fish_catch = pd.DataFrame({'id_own': bad_fish_catch.index, 'id_bad_fish':bad_fish_catch['div_sub_catch'].values})
    proverka_catch = catch[['id_fish','id_own','id_ves', 'date', 'catch_volume']]
    proverka_catch = proverka_catch[['id_ves','id_fish','id_own']].groupby(['id_ves'])
    proverka_catch = proverka_catch.aggregate(lambda x: x.unique().tolist())
    check = [] # Найденная аномальная Информация id_ves, date, catch_volume
    for i in range(len(bad_fish_catch)): #Проходим всех юр лиц len(bad_fish_catch)
        id_own = bad_fish_catch.iloc[i]['id_own'] #id_own - это один id со стороны изсветных
    
    for j in range(len(proverka_catch)): #Проходим все судна
        own = proverka_catch.iloc[j]['id_own'] #Набор принадлежностей судна к юр лицам

        if id_own in own: #Проверка принаждлежности судна к подозрительному юр лицу
            id_fish = bad_fish_catch.iloc[i]['id_bad_fish'] #id_fish - набор неправильной рыбы      
            fish = proverka_catch.iloc[j]['id_fish']

        for k_fish in id_fish: # Проверка добычи судна подозрительной рыбы
            if(k_fish in fish):
                ves = proverka_catch.index[j]
                check.append([ves, id_own, k_fish])

    df_check = pd.DataFrame(check)
    df_check.rename(columns={0:'id_ves', 1:'id_own', 2:'id_fish'}, inplace=True)

    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях vsd_absence\n', file=f)
        print(f'Выявлено {len(df_check)} аномалий (отсутствие ВСД на вылов)\n', file=f)
        print(f'Владельцам следующих кораблей были отправлены предупреждения:\n', file=f)
        print(df_check[['id_own', 'id_ves', 'id_fish']].sort_values(by=['id_own', 'id_ves']).value_counts().to_string(), file=f) 

    return df_check.to_json(), len(df_check)