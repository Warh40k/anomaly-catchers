def drop_dup_ext2(df):
    ext2 = df.copy()
    ext2['abs_error'] = 0
    tmp = ext2[ext2.duplicated('id_vsd', keep=False)]
    drop_idx = tmp.sort_values(by=['id_vsd', 'unit']).index[1::2]
    good_idx = tmp.sort_values(by=['id_vsd', 'unit']).index[::2]
    ext2.loc[drop_idx, 'abs_error'] = abs(ext2.loc[drop_idx, 'volume'].values * 1000 - ext2.loc[good_idx, 'volume'].values)
    tmp.sort_values(by=['id_vsd', 'date_vsd'])
    ext2.drop((tmp.loc[drop_idx, 'abs_error'] > 500).index).shape
    ext2 = ext2.drop((tmp.loc[drop_idx, 'abs_error'] > 500).index)
    return ext2

def drop_dup_ext1(df):
    ext1 = df.copy()
    tmp = ext1[ext1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves'])
    change_idx = ext1[ext1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves']).index
    ext1.loc[change_idx, 'Product_period'] = (tmp.Product_period + ' - ' + tmp.date_fishery.astype(str)).values
    drop_idx = ext1[ext1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves']).index
    ext1 = ext1.drop(drop_idx[1::2])
    return ext1



