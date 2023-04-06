import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { getPostBySlug, getPostComments } from '../../../services/posts';

import NotFound from '../../NotFound';
import PostContent from '../../../components/blog/PostContent';
import CommentForm from '../../../components/blog/CommentForm';
import PostComments from '../../../components/blog/PostComments';

export default function Post() {
	// Component's variables
	const params = useParams();

	// Component's states
	const [post, setPost] = useState({});
	const [comments, setComments] = useState([]);

	useEffect(() => {
		fetchPost();

		async function fetchPost() {
			const data = await getPostBySlug(params.slug);
			if (data) {
				setPost(data);
				const postComments = await getPostComments(data.id);
				if (postComments) setComments(postComments);
				else setComments([]);
			} else setPost({});
		}
	}, [params]);

	return (
		<>
			{post.id ? (
				<div className='p-4'>
					<PostContent post={post} />
					<hr />
					<CommentForm postId={post.id} />
					{comments.length > 0 && (
						<PostComments comments={comments} />
					)}
				</div>
			) : (
				<NotFound />
			)}
		</>
	);
}
