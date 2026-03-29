import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useTranslation } from 'react-i18next';
import { uploadChanges } from "../../../utils/ApiRequests";
import { AppPaths } from "../../../utils/AppPaths";
import { MANAGER } from "../../../utils/Constants";
import { verify } from "../../../utils/Navigation";
import GameForm from "./parts/GameForm";

const EditGame = () => {
    const navigate = useNavigate();
    useEffect(() => {
        if (!verify(MANAGER)) {
            navigate('/')
        }
    }, [navigate])

    const { t } = useTranslation();

    const saveChanges = async (game) => {
        console.log(game);
        const response = await uploadChanges(game);
        if(response.status === 200){
            toast.success(t('manager.editSuccess'));
            navigate(AppPaths.manager);
            return;
        }
        toast.error(t('messages.errorOccurred'));
    }

    return (
        <>
            {
                <GameForm saveChanges={saveChanges}/>
            }
        </>
    )
}
export default EditGame;