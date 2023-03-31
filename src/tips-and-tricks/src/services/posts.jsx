import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getPostsByQueries(queries) {
	const response = await axios.get(`${API_URL}/api/posts?${queries}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getPostBySlug(slug) {
	const response = await axios.get(`${API_URL}/api/posts/byslug/${slug}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getPostComments(id) {
	const response = await axios.get(`${API_URL}/api/posts/${id}/comments`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
