import React from "react";
import { useTranslation } from "react-i18next";
import CenteredContainer from "../../Containers/CenteredContainer";
const BitCoin = () => {
    const { t } = useTranslation();
    return (
        <CenteredContainer>
            <h3>{t('payment.methodUnavailable')}</h3>
        </CenteredContainer>
    )
}
export default BitCoin;