import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
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

    const saveChanges = async (newData) => {
        const response = await uploadChanges(newData);
        if(response.status === 200){
            toast.success('Гра успішно додана!');
            navigate(AppPaths.manager);
            return;
        }
        toast.error('Сталася помилка...');
    }

    return (
        <>
            <GameForm saveChanges={saveChanges} />
        </>
    )
}
export default CreateGame;