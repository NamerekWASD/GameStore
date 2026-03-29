import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useTranslation } from 'react-i18next';
import { uploadChanges } from "../../../utils/ApiRequests";
import { AppPaths } from "../../../utils/AppPaths";
import { MANAGER } from "../../../utils/Constants";
import { verify } from "../../../utils/Navigation";
import GameForm from "./parts/GameForm";

const CreateGame = () => {
    const navigate = useNavigate();

    useEffect(() => {
        if (!verify(MANAGER)) {
            navigate('/')
        }
    }, [navigate])

    const { t } = useTranslation();

    const saveChanges = async (newData) => {
        const response = await uploadChanges(newData);
        if(response.status === 200){
            toast.success(t('manager.createSuccess'));
            navigate(AppPaths.manager);
            return;
        }
        toast.error(t('messages.errorOccurred'));
    }

    return (
        <>
            <GameForm saveChanges={saveChanges} />
        </>
    )
}
export default CreateGame;