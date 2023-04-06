import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getTagBySlug(slug) {
	const { data } = await axios.get(`${API_URL}/tags/${slug}`);

	if (data.isSuccess) return data.result;
	else return null;
}
