import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
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

    const saveChanges = async (game) => {
        console.log(game);
        const response = await uploadChanges(game);
        if(response.status === 200){
            toast.success('Гра успішно змінена!');
            navigate(AppPaths.manager);
            return;
        }
        toast.error('Сталася помилка...');
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