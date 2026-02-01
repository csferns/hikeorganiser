import axios, {AxiosRequestConfig, AxiosResponse} from "axios";
import GetAllEvents from "../interfaces/GetAllEvents.ts";
import GetAllBucketLists from "../interfaces/GetAllBucketLists.ts";
import {Dayjs} from 'dayjs';
import ScheduleRequest from "../interfaces/ScheduleRequest.ts";
import AccessTokenResponse from "../interfaces/AccessTokenResponse.ts";

export default class ApiService {
    
    accessTokenResponse: AccessTokenResponse | undefined;
    
    constructor() {
        axios.defaults.baseURL = import.meta.env.VITE_REACT_APP_API_ENDPOINT
    }
        
    getAuthedConfig<T = any>(data : T | undefined = undefined) : AxiosRequestConfig<T> {
        
        // let t : string | null = this.accessTokenResponse?.accessToken || localStorage.getItem("token");
        //
        // if (!t) {
        //     throw new Error("No token provided");
        // }

        return {
            withCredentials: true,
            // headers:  {
            //     Authorization: `Bearer ${t}`
            // },
            data: data
        };
    }
    
    async login(email: string, password: string) : Promise<void> {
        const body = {
            email: email,
            password: password
        }

        const r = await axios.post('/auth/login?useCookies=true', body, { withCredentials: true });
        
        const d : AccessTokenResponse = r.data;
        
        this.accessTokenResponse = d;
        localStorage.setItem("token", d.accessToken);
        
        //setTimeout(() => this.refreshToken(d.refreshToken), d.expiresIn * 1000);
    }
    
    // async refreshToken(refreshToken: string) : Promise<void> {
    //     const body = {
    //         refreshToken: refreshToken
    //     };
    //
    //     let config = this.getAuthedConfig(body);
    //    
    //     const r =  await axios.post('/auth/refresh', config);
    //    
    //     this.accessTokenResponse = r.data;
    // }

    async register(email: string, password: string, referralCode: string | undefined) : Promise<void> {
        const body = {
            email: email,
            password: password,
            referralCode: referralCode
        }

        await axios.post('/auth/register?useCookies=true', body, { withCredentials: true });
        
        await this.login(email, password);
    }

    async logout() : Promise<void> {
        await axios.post('/auth/logout?useCookies=true', undefined, { withCredentials: true });
        this.accessTokenResponse = undefined;
        localStorage.removeItem("token");
    }

    async getUserInfo() : Promise<any> {
        const r = await axios.get('/auth/manage/fullinfo', { withCredentials: true });
        return r.data;
    }

    async updateProfile(profile: { email?: string; userName?: string; displayName?: string; }) : Promise<void> {
        const config = this.getAuthedConfig(profile);
        await axios.put('/auth/manage/profile', profile, { ...config, withCredentials: true });
    }

    async changePassword(currentPassword: string, newPassword: string) : Promise<void> {
        const body = { currentPassword, newPassword };
        const config = this.getAuthedConfig(body);
        await axios.put('/auth/manage/password', body, { ...config, withCredentials: true });
    }

    async getEvents(startDate: Dayjs, endDate: Dayjs) : Promise<GetAllEvents> {

        let config = this.getAuthedConfig();
        
        const r : AxiosResponse<GetAllEvents> = await axios.get(`/api/event?startDate=${startDate}&endDate=${endDate}`, config);

        return r.data;
    }
    
    async getBucketList(unplanned : boolean | null = null) : Promise<GetAllBucketLists> {
        
        let config = this.getAuthedConfig();

        const r : AxiosResponse<GetAllBucketLists> = await axios.get(unplanned  == null 
            ? '/api/bucketlist'
            : `/api/bucketlist?unplanned=${unplanned}`, config);

        return r.data;
    }
    
    async scheduleEvent(request : ScheduleRequest) : Promise<void> {

        let config = this.getAuthedConfig();
        
        return axios.put('/api/event/schedule', request, config);
    }
    
    async rsvp(eventId: number, status: number) {
        let config = this.getAuthedConfig();

        return axios.put(`/api/event/${eventId}/rsvp`, { status }, config);
    }
}

