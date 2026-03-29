import i18n from 'i18next';

export const MANAGER = 'manager'
export const POSTER = 'POSTER'


export const orderOptions = () => [
    { value: 0, label: i18n.t('constants.order.default') },
    { value: 1, label: i18n.t('constants.order.byDateDesc') },
    { value: 2, label: i18n.t('constants.order.byDateAsc') },
    { value: 3, label: i18n.t('constants.order.byPriceDesc') },
    { value: 4, label: i18n.t('constants.order.byPriceAsc') },
    { value: 5, label: i18n.t('constants.order.byPopularity') },
]