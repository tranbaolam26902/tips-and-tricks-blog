import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function subscribe(email) {
	const { data } = await axios.post(
		`${API_URL}/api/subscribers/${encodeURIComponent(email)}/subscribe`,
	);

	if (data.isSuccess) return true;
	else return data.errors;
}
