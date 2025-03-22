import { useState, useEffect } from 'react'
import { DatePicker } from '@fluentui/react';
import {
    Input,
    Field,
    useId,
    Button,
    Toaster,
    useToastController,
    Toast,
    ToastTitle,
    ToastBody,
    Checkbox,
    Select
} from "@fluentui/react-components";
import { SaveRegular } from "@fluentui/react-icons";
import axios from "axios";

import { DateType } from "../enums/DateType.ts";
import ScheduleRequest from "../interfaces/ScheduleRequest.ts";

export default function Schedule() {

    const weekDays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    const toasterId = useId("toaster");
    const { dispatchToast } = useToastController(toasterId);

    //const [schedule, setSchedule] = useState<ScheduleRequest>({});
    const [date, setDate] = useState<Date>(new Date());
    const [dateType, setDateType] = useState<DateType>(DateType.Manual);
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>();
    const [location, setLocation] = useState<string>();
    const [recurring, setRecurring] = useState<boolean>(false);

    const getPicker = () => {

        if (dateType & DateType.Manual) {
            return (<DatePicker value={date} onSelectDate={(d) => d && setDate(d)} />);
        }
        else if (dateType & DateType.Period) {

        }

        return (<></>);
    }

    const getDateDescription = () => {

        if (dateType == (DateType.Manual | DateType.Recurring)) {
            return `Weekly on ${weekDays[date.getDay()]} starting on ${date}`;
        }

        if (dateType == DateType.Manual) {
            return date.toString();
        }

        return "";
    }

    function Save() {

        const request : ScheduleRequest = {
            Title: title,
            Description: description,
            Location: location,
            DateType: dateType,
            StartDate: date,
            EndDate: undefined
        };

        console.log(request);
        // TODO: Send to the api

        //"https://localhost:7259"
        axios.put(`${process.env.REACT_APP_API_ENDPOINT}/schedule`, request)
            .then((_) => {
                dispatchToast(
                    <Toast>
                        <ToastTitle>Saved</ToastTitle>
                        <ToastBody>Saved {title}</ToastBody>
                    </Toast>,
                    { intent: "success" }
                )
            });
    }

    return (
        <>
            <div className={"row"}>
                <Field label="Title">
                    <Input value={title} onChange={(e) => setTitle(e.target.value)} />
                </Field>
                <Field label="Description">
                    <Input value={description} onChange={(e) => setDescription(e.target.value)} />
                </Field>
                <Field label="Location">
                    <Input value={location} onChange={(e) => setLocation(e.target.value)} />
                </Field>
                <Field label="Date">
                    <Select value={dateType} onChange={(e) => setDateType(e.target.value as keyof typeof DateType)}>
                        { Object.values(DateType).map(x => (<option value={x}>{x}</option>)) }
                    </Select>
                    <Checkbox label={"Recurring"} checked={recurring} onChange={(e) => setRecurring(e.target.checked)} />

                    {getPicker()}

                    {getDateDescription()}
                </Field>
            </div>

            <div className={"row"}>
                <Button icon={<SaveRegular />} appearance={"primary"} onClick={Save}>Save</Button>
            </div>

            <Toaster toasterId={toasterId} />
        </>
    )
}